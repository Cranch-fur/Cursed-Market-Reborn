﻿using Fiddler;

namespace Cursed_Market_Reborn
{
    public static class FiddlerCore
    {
        static FiddlerCore()
        {
            FiddlerApplication.BeforeRequest += FiddlerToCatchBeforeRequest;
            FiddlerApplication.AfterSessionComplete += FiddlerToCatchAfterSessionComplete;
        }
        private static bool EnsureRootCertificate()
        {
            if (!CertMaker.rootCertExists())
            {
                if (!CertMaker.createRootCert())
                    return false;
                if (!CertMaker.trustRootCert())
                    return false;
                FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.cert", null);
                FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.key", null);
            }
            return true;
        }
        public static void Start()
        {
            EnsureRootCertificate();
            CONFIG.IgnoreServerCertErrors = true;
            FiddlerApplication.Startup(new FiddlerCoreStartupSettingsBuilder().ListenOnPort(8888).RegisterAsSystemProxy().ChainToUpstreamGateway().DecryptSSL().OptimizeThreadPool().Build());
        }
        public static void Stop()
        {
            FiddlerApplication.Shutdown();
        }
        public static void FiddlerToCatchBeforeRequest(Session oSession)
        {
            if (oSession.uriContains("api/v1/config"))
            {
                Globals.FIDDLERCORE_VALUE_BHVRSESSION = oSession.oRequest["Cookie"].Replace("bhvrSession=", "");
                if (oSession.uriContains("steam.live"))
                    Globals.FIDDLERCORE_VALUE_PLATFORM = "steam.live";
                else if (oSession.uriContains("grdk.live"))
                    Globals.FIDDLERCORE_VALUE_PLATFORM = "grdk.live";
                return;
            }
            if (oSession.uriContains("api/v1/inventories"))
            {
                oSession.utilCreateResponseAndBypassServer();
                oSession.utilSetResponseBody(Globals.FIDDLERCORE_VALUE_MARKETFILE);
                return;
            }
            if (Globals.FIDDLERCORE_BOOL_ANTICHATFILTER == true)
            {
                if (oSession.uriContains("api/v1/profanityfilter/sanitizer/message"))
                {
                    oSession.fullUrl = "http://api.dragonwild.ru/v1/fuckFilter";
                }
            }
            if (Globals.FIDDLERCORE_BOOL_SILENTFULLPROFILE == true)
            {
                if (oSession.uriContains("api/v1/players/me/states/FullProfile/binary"))
                {
                    oSession.utilCreateResponseAndBypassServer();
                    oSession.oResponse["Content-Type"] = "application/octet-stream";
                    oSession.oResponse["Kraken-State-Version"] = "1";
                    oSession.oResponse["Kraken-State-Schema-Version"] = "0";
                    oSession.utilSetResponseBody(Globals.FIDDLERCORE_VALUETRANSFER_FULLPROFILE());
                }
                if (oSession.uriContains("api/v1/players/me/states/binary?schemaVersion"))
                {
                    oSession.utilCreateResponseAndBypassServer();
                    oSession.utilSetResponseBody("{\"version\":1,\"stateName\":\"FullProfile\",\"schemaVersion\":0,\"playerId\":\"0cfddbd9-4738-d130-aa5e-6e4f165b4440\"}");
                }
            }
            if (Globals.FIDDLERCORE_BOOL_CURRENCYSPOOF == true)
            {
                if (oSession.uriContains("api/v1/wallet/currencies"))
                {
                    oSession.utilCreateResponseAndBypassServer();
                    oSession.utilSetResponseBody("{\"list\":[{\"balance\":" + Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_SHARDS + ",\"currency\":\"Shards\"},{\"balance\":" + Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_CELLS + ",\"currency\":\"Cells\"},{\"balance\":" + Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_BLOODPOINTS + ",\"currency\":\"BonusBloodpoints\"},{\"balance\":0,\"currency\":\"Bloodpoints\"}]}");
                }
            }
        }
        public static void FiddlerToCatchAfterSessionComplete(Session oSession)
        {
            if (oSession.uriContains("api/v1/queue"))
            {
                oSession.utilDecodeResponse();
                Globals.FIDDLERCORE_VALUE_QUEUEPOSITION = System.Text.Encoding.UTF8.GetString(oSession.responseBodyBytes);
            }
            if (oSession.uriContains("api/v1/match"))
            {
                oSession.utilDecodeResponse();
                Globals.FIDDLERCORE_VALUE_QUEUEPOSITION = null;
            }
            if (oSession.uriContains("api/v1/auth/provider/steam/login?token="))
            {
                Globals.FIDDLERCORE_VALUE_UID = Globals.FIDDLERCORE_VALUETRANSFER_UID(oSession.url.ToString());
            }
        }
    }
}
