using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace LyoServer
{
    public interface IHttpHandler
    {
        void ProcessRequest(HttpContext context);
    }

    public class HttpApplication : IHttpHandler
    {
        private string webStationName;

        public string WebStationName
        {
            get { return webStationName; }
            set { webStationName = value; }
        }

        // 对请求上下文进行处理
        public void ProcessRequest(HttpContext context)
        {
            if(string.IsNullOrEmpty(webStationName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("网站未正确配置，请检查！");
                return;
            }
            // 1.获取网站根路径
            string bastPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.Combine(bastPath + "\\" + webStationName, context.Request.Url.TrimStart('/'));
            string fileExtension = Path.GetExtension(context.Request.Url);
            // 2.处理动态文件请求
            if (fileExtension.Equals(".aspx") || fileExtension.Equals(".ashx"))
            {
                string className = Path.GetFileNameWithoutExtension(context.Request.Url);
                System.Web.IHttpHandler handler = Assembly.Load(webStationName).CreateInstance(webStationName + "." + className) as System.Web.IHttpHandler;


                var request = context.Request;
                var response = context.Response;
                System.Web.HttpRequest req = new System.Web.HttpRequest("", request.Url,"");

                //var webcontext = new System.Web.HttpContext(request, response);
                //handler.ProcessRequest(webcontext);

                //var cnt = (System.Web.HttpContext)context;
                //handler.ProcessRequest(cnt);

                //IHttpHandler handler = instance as IHttpHandler;
                //handler.ProcessRequest(context);

                return;
            }
            // 3.处理静态文件请求
            if (!File.Exists(fileName))
            {
                context.Response.StateCode = "404";
                context.Response.StateDescription = "Not Found";
                context.Response.ContentType = "text/html";
                string notExistHtml = Path.Combine(bastPath, webStationName + @"\404.html");
                context.Response.Body = File.ReadAllBytes(notExistHtml);
            }
            else
            {
                context.Response.StateCode = "200";
                context.Response.StateDescription = "OK";
                context.Response.ContentType = GetContenType(Path.GetExtension(context.Request.Url));
                context.Response.Body = File.ReadAllBytes(fileName);
            }
        }

        // 根据文件扩展名获取内容类型
        public string GetContenType(string fileExtension)
        {
            string type = "text/html; charset=UTF-8";
            switch (fileExtension)
            {
                case ".aspx":
                case ".html":
                case ".htm":
                    type = "text/html; charset=UTF-8";
                    break;
                case ".png":
                    type = "image/png";
                    break;
                case ".gif":
                    type = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    type = "image/jpeg";
                    break;
                case ".css":
                    type = "text/css";
                    break;
                case ".js":
                    type = "application/x-javascript";
                    break;
                default:
                    type = "text/plain; charset=gbk";
                    break;
            }
            return type;
        }
    }
}
