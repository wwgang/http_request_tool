using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentTools
{
    public class HttpHelper
    {
        static public async Task Post(string url, Dictionary<string, string> headers, string body, DecompressionMethods decompress = DecompressionMethods.GZip, Action<string> callback = null)
        {
            try
            {
                var header = GetObj(headers);

                var handler = new HttpClientHandler() { UseCookies = false, AutomaticDecompression = decompress };
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(body, Encoding.UTF8, header.ContentType)
                };
                SetHead(req.Headers, headers);
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("User-Agent", header.UserAgent);
                client.DefaultRequestHeaders.Add("Referer", header.Referer);
                client.DefaultRequestHeaders.Add("Origin", header.Origin);
                client.DefaultRequestHeaders.Add("Accept-Language", header.AcceptLanguage);
                client.DefaultRequestHeaders.Connection.Add(header.Connection);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Accept));

                var res = await client.SendAsync(req);
                var str = await res.Content.ReadAsStringAsync();

                if (callback != null)
                    callback(str);
            }
            catch (Exception ex)
            {
                if (callback != null)
                    callback(ex.Message);
            }
        }

        static public async Task Get(string url, Dictionary<string, string> headers, DecompressionMethods decompress = DecompressionMethods.GZip, Action<string> callback = null)
        {
            try
            {
                var header = GetObj(headers);
                var handler = new HttpClientHandler() { UseCookies = false, AutomaticDecompression = decompress };
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url);
                SetHead(req.Headers, headers);
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("User-Agent", header.UserAgent);
                var res = await client.SendAsync(req);
                var str = await res.Content.ReadAsStringAsync();

                if (callback != null)
                    callback(str);
            }
            catch (Exception ex)
            {
                if (callback != null)
                    callback(ex.Message);
            }
        }

        private static void SetHead(HttpRequestHeaders headers, Dictionary<string, string> dics)
        {
            foreach (KeyValuePair<string, string> item in dics)
            {
                if (item.Key == "Content-Type") continue;
                if (item.Key == "Referer") continue;
                if (item.Key == "Content-Length") continue;
                if (item.Key == "Accept") continue;
                if (item.Key == "Connection") continue;
                if (item.Key == "Host") continue;

                headers.Add(item.Key, item.Value);
            }
        }

        private static HeaderObj GetObj(Dictionary<string, string> headers)
        {
            HeaderObj obj = new HeaderObj();
            if (headers.ContainsKey("Content-Type"))
            {
                obj.ContentType = headers["Content-Type"];
            }
            if (headers.ContainsKey("Referer"))
            {
                obj.ContentType = headers["Referer"];
            }
            if (headers.ContainsKey("Connection"))
            {
                obj.ContentType = headers["Connection"];
            }
            if (headers.ContainsKey("Accept"))
            {
                obj.ContentType = headers["Accept"];
            }
            if (headers.ContainsKey("User-Agent"))
            {
                obj.ContentType = headers["User-Agent"];
            }
            if (headers.ContainsKey("Origin"))
            {
                obj.ContentType = headers["Origin"];
            }
            if (headers.ContainsKey("Accept-Language"))
            {
                obj.ContentType = headers["Accept-Language"];
            }
            return obj;
        }
    }

    class HeaderObj
    {
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";
        public string UserAgent { get; set; } = "Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.87 Mobile Safari/537.36";
        public string Referer { get; set; }
        public string Connection { get; set; }
        public string Origin { get; set; }
        public string AcceptLanguage { get; set; }
        public string Accept { get; set; }
    }
}
