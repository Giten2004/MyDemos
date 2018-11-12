using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jira
{
    /// <summary>
    /// 模拟网页操作,提交、获取订单页面数据
    /// </summary>
    public class HttpWebRequestExtension
    {
        private static string ContentType = "application/x-www-form-urlencoded";
        private static string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private static string UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        private static string Referer = "http://jira.liquid-capital.liquidcap.com/login.jsp";
        
        /// <summary>
        /// 提交订单数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string PostWebContent(string url, CookieContainer cookie, string param)
        {
            byte[] bs = Encoding.ASCII.GetBytes(param);

            var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.CookieContainer = cookie;
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = Accept;
            httpWebRequest.UserAgent = UserAgent;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = bs.Length;

            using (Stream reqStream = httpWebRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string html = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();

            httpWebRequest.Abort();
            httpWebResponse.Close();

            return html;
        }

        /// <summary>
        /// 获取页面数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string GetWebContent(string url, CookieContainer cookie)
        {
            var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.CookieContainer = cookie;
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Referer = Referer;
            httpWebRequest.Accept = Accept;
            httpWebRequest.UserAgent = UserAgent;
            httpWebRequest.Method = "GET";
            httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;

            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string html = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();

            httpWebRequest.Abort();
            httpWebResponse.Close();

            return html;
        }

        /// <summary>
        /// 获取网页验证码图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static object GetWebImage(string url, CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Referer = Referer;
            request.UserAgent = UserAgent;
            request.Accept = "image/webp,*/*;q=0.8";
            request.CookieContainer = cookie;
            request.ContentType = ContentType;
            request.KeepAlive = true;
            request.UseDefaultCredentials = true;
            //  request.Proxy = null;
            return request.GetResponse().GetResponseStream();
        }
    }
}
