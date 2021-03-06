using CranchyLib.SaveFile;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Cursed_Market_Reborn
{
    public static class Globals
    {
        ///////////////////////////////// => High Priority Variables
        public static string PROGRAM_EXECUTABLE = System.AppDomain.CurrentDomain.FriendlyName;
        public static string PROGRAM_RANDOMNAME()
        {
            Random rnd = new Random();
            byte[] array = new byte[rnd.Next(8, 16)];
            rnd.NextBytes(array);
            return System.Convert.ToBase64String(array);
        }

        public const string REGISTRY_MAIN = @"HKEY_CURRENT_USER\SOFTWARE\Cursed Market";
        public const string PROGRAM_OFFLINEVERSION = "3801";
        public static string PROGRAM_TEXT_OFFLINEVERSION = System.Text.RegularExpressions.Regex.Replace(PROGRAM_OFFLINEVERSION, "(.)", "$1.").Remove(PROGRAM_OFFLINEVERSION.Length * 2 - 1);
        public static string PROGRAM_NEWS_LASTSEENTHREAD = REGISTRY_GETVALUE("LSNT");

        public static string REGISTRY_VALUE_PAKFILEPATH = REGISTRY_GETVALUE("PakFilePath");
        public static string REGISTRY_VALUE_PAKFOLDERPATH = REGISTRY_VALUE_PAKFILEPATH.Replace("\\pakchunk1-WindowsNoEditor.pak", "");
        public static string REGISTRY_VALUE_THEME = REGISTRY_GETVALUE("SelectedTheme");
        public static int INITIALIZEDTHEME = 0;
        public static string REGISTRY_GETVALUE(string WINREGNAME)
        {
            try
            { return Registry.GetValue(REGISTRY_MAIN, WINREGNAME, "NONE").ToString(); }
            catch { return "NONE"; }
        }
        public static int REGISTRY_GETINTVALUE(string WINREGNAME)
        {
            try
            { return Convert.ToInt32(Registry.GetValue(REGISTRY_MAIN, WINREGNAME, -1)); }
            catch { return -1; }
        }

        public static string OVERRIDEN_VALUE_USERAGENT = null;

        public static string FIDDLERCORE_VALUE_PLATFORM = null;
        public static string FIDDLERCORE_VALUE_MARKETFILE = null;
        public static string FIDDLERCORE_VALUE_ADVANCEDSKINCONTROL = null;
        public static string FIDDLERCORE_VALUE_SEASONMANAGER = null;
        public static string FIDDLERCORE_VALUE_FULLPROFILE = null;
        public static string FIDDLERCORE_VALUE_BHVRSESSION = null;
        public static string FIDDLERCORE_VALUE_QUEUEPOSITION = "NONE";
        public static string FIDDLERCORE_VALUE_MMR = string.Empty;
        public static string FIDDLERCORE_VALUE_CURRENTMATCHID = null;
        public static string FIDDLERCORE_VALUE_UID = null;
        public static string FIDDLERCORE_VALUE_PID = null;
        public static string FIDDLERCORE_VALUETRANSFER_QUEUEPOSITION(string input)
        {
            if (FIDDLERCORE_VALUE_QUEUEPOSITION != null)
            {
                try
                {
                    var JsQueueResponse = JObject.Parse(input);
                    if ((string)JsQueueResponse["status"] == "QUEUED")
                        return (string)JsQueueResponse["queueData"]["position"];
                    else if ((string)JsQueueResponse["status"] == "MATCHED")
                    {
                        FIDDLERCORE_VALUE_CURRENTMATCHID = (string)JsQueueResponse["matchData"]["matchId"];
                        Overlay.IsMMRObtained = false;
                        return "MATCHED";
                    }
                    else
                        return "NONE";
                }
                catch { return "NONE"; }
            }
            else return "NONE";
        }
        public static string FIDDLERCORE_VALUETRANSFER_MMR(string input)
        {
            try
            {
                if (FIDDLERCORE_VALUE_MMR != null)
                {
                    var JsMatchResponse = JObject.Parse(input);
                    if ((float)JsMatchResponse["skill"]["rating"]["rating"] > 0)
                        return Convert.ToString(Convert.ToInt32((float)JsMatchResponse["skill"]["rating"]["rating"]));
                    else
                        return "NONEVALUE";
                }
                else return "NONEVALUE";
            }
            catch { return "NONEVALUE"; }
        }
        public static string FIDDLERCORE_VALUETRANSFER_UID(string input)
        {
            if (input != null)
            {
                string output = input.Remove(0, input.IndexOf("14", StringComparison.InvariantCulture)).Remove(0, 24);
                output = output.Remove(16, output.Length - 16);
                return output;
            }
            else return null;
        }
        public static string FIDDLERCORE_VALUETRANSFER_FULLPROFILE()
        {
            if (FIDDLERCORE_VALUE_FULLPROFILE != null)
            {
                dynamic JsSaveFile = JsonConvert.DeserializeObject(FIDDLERCORE_VALUE_FULLPROFILE);
                JsSaveFile["playerUId"] = FIDDLERCORE_VALUE_UID;
                JsSaveFile["currentSeasonTicks"] = (long)((DateTime.Now.ToUniversalTime() - CurrentNETtimestampstart).TotalMilliseconds + 0.5);
                return SaveFile.EncryptSavefile(JsonConvert.SerializeObject(JsSaveFile, Formatting.None));
            }
            else return null;
        }

        public static string FIDDLERCORE_VALUETRANSFER_MARKETFILE()
        {
            if (FIDDLERCORE_VALUE_MARKETFILE != null)
            {
                try
                {
                    JObject JsMarketFile = JObject.Parse(FIDDLERCORE_VALUE_MARKETFILE);
                    JObject JsMarketFileData = (JObject)JsMarketFile.SelectToken("data");
                    JsMarketFileData.Property("playerId").Value = FIDDLERCORE_VALUE_PID;
                    return JsMarketFile.ToString();
                }
                catch { return FIDDLERCORE_VALUE_MARKETFILE; }
            }
            else return null;
        }

        public static bool FIDDLERCORE_BOOL_ISADVANCEDSKINCONTROLENABLED = false;
        public static bool FIDDLERCORE_BOOL_SEASONMANAGER = false;
        public static bool FIDDLERCORE_BOOL_SILENTFULLPROFILE = false;
        public static bool FIDDLERCORE_BOOL_ANTIBOTMATCH = false;
        public static bool FIDDLERCORE_BOOL_CURRENCYSPOOF = false;
        public static bool FIDDLERCORE_BOOL_FREEBLOODWEB = false;
        public static string FIDDLERCORE_VALUE_CURRENCYSPOOF_BLOODPOINTS = null;
        public static string FIDDLERCORE_VALUE_CURRENCYSPOOF_SHARDS = null;
        public static string FIDDLERCORE_VALUE_CURRENCYSPOOF_CELLS = null;

        public static string SSL_SHIPPING, SSL_PTB;

        public static DateTime CurrentNETtimestampstart { get; private set; }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.ASCII.GetString(base64EncodedBytes);
        }

        public static string CROSSHAIR_VALUE_SELECTEDCROSSHAIR = REGISTRY_GETVALUE("SelectedCrosshair");
        public static int CROSSHAIR_VALUE_OPACITY = REGISTRY_GETINTVALUE("CrosshairOpacity");

        public static void DisableProxy()
        {
            try
            {
                string regproxykeypath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";
                Registry.SetValue(regproxykeypath, "ProxyEnable", 0);
                Registry.SetValue(regproxykeypath, "ProxyServer", "");
                Registry.SetValue(regproxykeypath, "ProxyOverride", "");
            }
            catch { }
        }

        public static void MATCHINFORMER_VALUETRANSFER_FIDDLERCOREMATCHDATA(string input)
        {
            try
            {
                var JsMatchData = JObject.Parse(input);
                string matchId = (string)JsMatchData["matchId"];
                string characterName = (string)JsMatchData["props"]["characterName"];

                bool isKillerObtained = false;
                switch (characterName)
                {
                    case "Chuckles":
                        isKillerObtained = true;
                        break;

                    case "Bob":
                        isKillerObtained = true;
                        break;

                    case "HillBilly":
                        isKillerObtained = true;
                        break;

                    case "Nurse":
                        isKillerObtained = true;
                        break;

                    case "Shape":
                        isKillerObtained = true;
                        break;

                    case "Witch":
                        isKillerObtained = true;
                        break;

                    case "Killer07":
                        isKillerObtained = true;
                        break;

                    case "Bear":
                        isKillerObtained = true;
                        break;

                    case "Cannibal":
                        isKillerObtained = true;
                        break;

                    case "Nightmare":
                        isKillerObtained = true;
                        break;

                    case "Pig":
                        isKillerObtained = true;
                        break;

                    case "Clown":
                        isKillerObtained = true;
                        break;

                    case "Spirit":
                        isKillerObtained = true;
                        break;

                    case "Legion":
                        isKillerObtained = true;
                        break;

                    case "Plague":
                        isKillerObtained = true;
                        break;

                    case "Ghostface":
                        isKillerObtained = true;
                        break;

                    case "Demogorgon":
                        isKillerObtained = true;
                        break;

                    case "Oni":
                        isKillerObtained = true;
                        break;

                    case "Gunslinger":
                        isKillerObtained = true;
                        break;

                    case "K20":
                        isKillerObtained = true;
                        break;

                    case "K21":
                        isKillerObtained = true;
                        break;

                    case "K22":
                        isKillerObtained = true;
                        break;

                    case "K23":
                        isKillerObtained = true;
                        break;

                    case "K24":
                        isKillerObtained = true;
                        break;

                    case "K25":
                        isKillerObtained = true;
                        break;
                }

                if (isKillerObtained == true)
                    MatchInformer.FiddlerCoreObtainedMatchInfo = matchId + ";" + characterName;
                else
                    MatchInformer.FiddlerCoreObtainedMatchInfo = null;

            } 
            catch { MatchInformer.FiddlerCoreObtainedMatchInfo = null; }
        }
    }
}
