using System;
using System.Collections.Generic;
using System.Text;

namespace LyoServer
{
    public class HttpContext
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }

        public HttpContext(string requestText)
        {
            Request = new HttpRequest(requestText);
            Response = new HttpResponse();
        }
    }

    public class HttpRequest
    {
        public HttpRequest(string requestText)
        {
            string[] lines = requestText.Replace("\r\n", "\r").Split('\r');
            string[] requestLines = lines[0].Split(' ');
            // 获取HTTP请求方式、请求的URL地址、HTTP协议版本
            HttpMethod = requestLines[0];
            Url = requestLines[1];
            HttpVersion = requestLines[2];
        }
        // 请求方式：GET or POST?
        public string HttpMethod { get; set; }
        // 请求URL
        public string Url { get; set; }
        // Http协议版本
        public string HttpVersion { get; set; }
        // 请求头
        public Dictionary<string, string> HeaderDictionary { get; set; }
        // 请求体
        public Dictionary<string, string> BodyDictionary { get; set; }
    }

    public class HttpResponse
    {
        // 响应状态码
        public string StateCode { get; set; }
        // 响应状态描述
        public string StateDescription { get; set; }
        // 响应内容类型
        public string ContentType { get; set; }
        //响应报文的正文内容
        public byte[] Body { get; set; }

        // 生成响应头信息
        public byte[] GetResponseHeader()
        {
            string strRequestHeader = string.Format(@"HTTP/1.1 {0} {1}
Content-Type: {2}
Accept-Ranges: bytes
Server: Microsoft-IIS/7.5
X-Powered-By: ASP.NET
Date: {3} 
Content-Length: {4}

", StateCode, StateDescription, ContentType, string.Format("{0:R}", DateTime.Now), Body.Length);

            return Encoding.UTF8.GetBytes(strRequestHeader);
        }
    }
}
