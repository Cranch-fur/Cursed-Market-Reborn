using Fiddler;

namespace Cursed_Market_Reborn
{
    public static class FiddlerCore
    {
        static FiddlerCore()
        {
            FiddlerApplication.BeforeRequest += FiddlerToCatchBeforeRequest;
            FiddlerApplication.AfterSessionComplete += FiddlerToCatchAfterSessionComplete;
            FiddlerApplication.ResetSessionCounter();
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
        public static bool RemoveRootCertificate()
        {
            try
            {
                CertMaker.removeFiddlerGeneratedCerts(true);
                return true;
            }
            catch { return false; }
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
            Globals.DisableProxy();
        }
        public static void FiddlerToCatchBeforeRequest(Session oSession)
        {
            if (oSession.hostname == "steam.live.bhvrdbd.com" || oSession.hostname == "grdk.live.bhvrdbd.com" || oSession.hostname == "cdn.live.dbd.bhvronline.com" || oSession.hostname == "latest.ptb.bhvrdbd.com" || oSession.hostname == "cdn.ptb.dbd.bhvronline.com")
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

                if(oSession.uriContains("api/v1/friends/richPresence"))
                {
                    Globals.FIDDLERCORE_VALUE_PID = oSession.url.Remove(0, oSession.url.LastIndexOf("/") + 1);
                    return;
                }

                if (oSession.uriContains("api/v1/inventories"))
                {
                    if (oSession.oRequest["x-kraken-client-os"] != "21.7.28700.1.768.64bit")
                    {
                        oSession.utilCreateResponseAndBypassServer();
                        oSession.utilSetResponseBody(Globals.FIDDLERCORE_VALUETRANSFER_MARKETFILE());
                        NetServices.REQUEST_DBDEMULATION(oSession.url);
                    }
                    
                    return;
                }

                if (Globals.FIDDLERCORE_BOOL_ANTIBOTMATCH == true)
                {
                    if (oSession.uriContains("api/v1/onboarding/get-bot-match-status"))
                    {
                        oSession.utilCreateResponseAndBypassServer();
                        oSession.utilSetResponseBody("{\"survivorMatchPlayed\":true,\"killerMatchPlayed\":true}");
                        return;
                    }
                }

                if (Globals.FIDDLERCORE_BOOL_SILENTFULLPROFILE == true)
                {
                    if (oSession.uriContains("api/v1/players/me/states/FullProfile/binary"))
                    {
                        if (oSession.oRequest["x-kraken-client-os"] != "21.7.28700.1.768.64bit")
                        {
                            oSession.utilCreateResponseAndBypassServer();
                            oSession.oResponse["Content-Type"] = "application/octet-stream";
                            oSession.oResponse["Kraken-State-Version"] = "1";
                            oSession.oResponse["Kraken-State-Schema-Version"] = "0";
                            oSession.utilSetResponseBody(Globals.FIDDLERCORE_VALUETRANSFER_FULLPROFILE());
                            NetServices.REQUEST_DBDEMULATION(oSession.url);
                        }

                        return;
                    }
                    if (oSession.uriContains("api/v1/players/me/states/binary?schemaVersion"))
                    {
                        oSession.utilCreateResponseAndBypassServer();
                        oSession.utilSetResponseBody("{\"version\":1,\"stateName\":\"FullProfile\",\"schemaVersion\":0,\"playerId\":\"" + Globals.FIDDLERCORE_VALUE_PID + "\"}");
                        return;
                    }
                }

                if (Globals.FIDDLERCORE_BOOL_CURRENCYSPOOF == true)
                {
                    if (oSession.uriContains("api/v1/wallet/currencies"))
                    {
                        if (oSession.oRequest["x-kraken-client-os"] != "21.7.28700.1.768.64bit")
                        {
                            oSession.utilCreateResponseAndBypassServer();
                            oSession.utilSetResponseBody("{\"list\":[{\"balance\":" + Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_SHARDS + ",\"currency\":\"Shards\"},{\"balance\":" + Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_CELLS + ",\"currency\":\"Cells\"},{\"balance\":" + Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_BLOODPOINTS + ",\"currency\":\"BonusBloodpoints\"},{\"balance\":0,\"currency\":\"Bloodpoints\"}]}");
                            NetServices.REQUEST_DBDEMULATION(oSession.url);
                        }

                        return;
                    }
                }

                if (Globals.FIDDLERCORE_BOOL_FREEBLOODWEB == true)
                {
                    if (oSession.uriContains("v1/wallet/withdraw"))
                    {
                        oSession.utilCreateResponseAndBypassServer();
                        oSession.utilSetResponseBody("{\"userId\":\"null\",\"balance\":0,\"currency\":\"USCents\"}");
                        return;
                    }
                }

                if (Globals.FIDDLERCORE_BOOL_ISADVANCEDSKINCONTROLENABLED == true)
                {
                    if (oSession.hostname == "cdn.ptb.dbd.bhvronline.com")
                        return;

                    if (oSession.uriContains("/catalog.json"))
                    {
                        oSession.utilCreateResponseAndBypassServer();
                        oSession.utilSetResponseBody(Globals.FIDDLERCORE_VALUE_ADVANCEDSKINCONTROL);
                        return;
                    }
                }

                if (Globals.FIDDLERCORE_BOOL_SEASONMANAGER == true)
                {
                    if (oSession.hostname == "cdn.ptb.dbd.bhvronline.com")
                        return;

                    if (oSession.uriContains("/specialEventsContent.json"))
                    {
                        oSession.utilCreateResponseAndBypassServer();
                        oSession.utilSetResponseBody(Globals.FIDDLERCORE_VALUE_SEASONMANAGER);
                        return;
                    }
                }
            }
            else return;
        }

        public static void FiddlerToCatchAfterSessionComplete(Session oSession)
        {
            if (oSession.hostname == "steam.live.bhvrdbd.com" || oSession.hostname == "grdk.live.bhvrdbd.com" || oSession.hostname == "latest.ptb.bhvrdbd.com")
            {
                if (oSession.uriContains("api/v1/queue"))
                {
                    try
                    {
                        if (oSession.uriContains("/cancel"))
                            Globals.FIDDLERCORE_VALUE_QUEUEPOSITION = "NONE";
                        else
                        {
                            if(Overlay.IsMatchFound)
                                Overlay.IsMatchFound = false;

                            oSession.utilDecodeResponse();
                            Globals.FIDDLERCORE_VALUE_QUEUEPOSITION = Globals.FIDDLERCORE_VALUETRANSFER_QUEUEPOSITION(System.Text.Encoding.UTF8.GetString(oSession.responseBodyBytes));
                        }
                        return;
                    } catch { Globals.FIDDLERCORE_VALUE_QUEUEPOSITION = "NONE"; return; }
                }
                if (oSession.uriContains("api/v1/match"))
                {
                    Globals.FIDDLERCORE_VALUE_QUEUEPOSITION = "MATCHED";
                    oSession.utilDecodeResponse();
                    string responseAsString = System.Text.Encoding.UTF8.GetString(oSession.responseBodyBytes);
                    Globals.FIDDLERCORE_VALUE_MMR = Globals.FIDDLERCORE_VALUETRANSFER_MMR(responseAsString);
                    Globals.MATCHINFORMER_VALUETRANSFER_FIDDLERCOREMATCHDATA(responseAsString);
                    return;
                }
                if (oSession.uriContains("api/v1/auth/provider/steam/login?token="))
                {
                    Globals.FIDDLERCORE_VALUE_UID = Globals.FIDDLERCORE_VALUETRANSFER_UID(oSession.url.ToString());
                    return;
                }
            }
            else return;
        }
    }
}
