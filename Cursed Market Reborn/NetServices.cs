using System;
using System.IO;
using System.Net;

namespace Cursed_Market_Reborn
{
    public static class NetServices
    {
        public static string REQUEST_GET(string URL, string variable, string variable2)
        {
            try
            {
                if (variable != "")
                    URL = URL + $"?{variable}";
                if (variable2 != "")
                    URL = URL + $"&{variable2}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 7000;
                request.ServicePoint.Expect100Continue = true;

                request.UserAgent = "Cursed Market";
                request.ContentType = "application/json";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch { return "ERROR"; }
        }
        public static void REQUEST_DBDEMULATION(string URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + URL);
                request.Timeout = 7000;
                request.ServicePoint.Expect100Continue = true;

                if(Globals.OVERRIDEN_VALUE_USERAGENT != null)
                    request.UserAgent = Globals.OVERRIDEN_VALUE_USERAGENT;
                else request.UserAgent = "DeadByDaylight/++DeadByDaylight+Live-CL-464461 Windows/10.0.22000.1.768.64bit";

                request.Headers.Add("Cookie", "bhvrSession=" + Globals.FIDDLERCORE_VALUE_BHVRSESSION);
                request.Headers.Add("x-kraken-client-version", "5.2.2");
                request.Headers.Add("x-kraken-client-provider", "steam");
                request.Headers.Add("x-kraken-client-platform", "steam");
                request.Headers.Add("x-kraken-client-os", "21.7.28700.1.768.64bit");
                request.ContentType = "application/json";

                request.GetResponse();
            }
            catch { }
        }
        public static string REQUEST_POST(string URL, string variable, string useragent, string cookies, string content)
        {
            try
            {
                if (variable != "")
                    URL = URL + $"?{variable}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 7000;
                request.ServicePoint.Expect100Continue = true;

                if (useragent != "")
                    request.UserAgent = useragent;
                else
                    request.UserAgent = "Cursed Market";

                if (cookies != "")
                    request.Headers.Add("Cookie", cookies);

                request.ContentType = "application/json";
                request.Method = "POST";

                using(Stream requestStream = request.GetRequestStream())
                {
                    byte[] requestAsByteArray = System.Text.Encoding.UTF8.GetBytes(content);
                    requestStream.Write(requestAsByteArray, 0, requestAsByteArray.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch { return "ERROR"; }
        }
        public static bool REQUEST_DOWNLOAD(string URL, string outputfilepath)
        {
            try
            {
                using (WebClient Web = new WebClient())
                {
                    if (File.Exists(outputfilepath))
                        File.Delete(outputfilepath);
                    Web.DownloadFile(URL, outputfilepath);
                    return true;
                }
            } catch { return false; }
        }
    }
}
