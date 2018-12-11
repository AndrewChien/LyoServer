using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LyoServer
{
    public class ServerKernel
    {
        private Socket socketWatch;
        private Thread threadWatch;
        private bool isEndService;
        private string WebStationName;
        public void StartServer()
        {
            var ip = Config.CreatInstance().GetConfigValue("ip");
            var port = Config.CreatInstance().GetConfigValue("port");
            // 创建Socket->绑定IP与端口->设置监听队列的长度->开启监听连接
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketWatch.Bind(new IPEndPoint(IPAddress.Parse(ip), int.Parse(port)));
            socketWatch.Listen(10);
            // 创建Thread->后台执行
            WebStationName = Config.CreatInstance().GetConfigValue("webstation");//设置启动网站
            threadWatch = new Thread(ListenClientConnect);
            threadWatch.IsBackground = true;
            threadWatch.Start(socketWatch);
            isEndService = false;
            Console.Title = "LyoServer服务器v1.0";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LyoServer服务已启动，正在监听 https://" + ip + ":" + port + "/ 端口...");
        }

        private void ListenClientConnect(object obj)
        {
            Socket socketListen = obj as Socket;
            while (!isEndService)
            {
                Socket proxSocket = socketListen.Accept();
                byte[] data = new byte[1024 * 1024 * 2];
                int length = proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                // Step1:接收HTTP请求
                string requestText = Encoding.Default.GetString(data, 0, length);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(requestText);
                HttpContext context = new HttpContext(requestText);
                // Step2:处理HTTP请求
                HttpApplication application = new HttpApplication();
                application.WebStationName = WebStationName;
                application.ProcessRequest(context);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(string.Format("{0} {1} from {2}", context.Request.HttpMethod, context.Request.Url, proxSocket.RemoteEndPoint.ToString()));
                // Step3:响应HTTP请求
                proxSocket.Send(context.Response.GetResponseHeader());
                proxSocket.Send(context.Response.Body);
                // Step4:即时关闭Socket连接
                proxSocket.Shutdown(SocketShutdown.Both);
                proxSocket.Close();
            }
        }

        public void StopServer()
        {
            if (socketWatch.Connected)
            {
                // 正常退出连接
                socketWatch.Shutdown(SocketShutdown.Both);
                // 释放相关资源
                socketWatch.Close();
            }
        }
    }
}
