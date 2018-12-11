using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LyoServer
{
    public class WebServer
    {
        private Socket _serverSocket;

        private LyoHost _host;

        public int Port { get; private set; }

        public bool IsRuning { get; set; }

        public WebServer(LyoHost host, int port)
        {
            _host = host;
            Port = port;
        }

        public void Start()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.ExclusiveAddressUse = true;
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
            _serverSocket.Listen(1000);
            IsRuning = true;

            Console.Title = "LyoServer WEB服务器 v1.0";
            Console.WriteLine("Serving HTTP on 0.0.0.0 port " + Port + " Started , Now listening...");

            new Thread(OnStart).Start();
        }

        private void OnStart(object state)
        {
            while (IsRuning)
            {
                try
                {
                    Socket socket = _serverSocket.Accept();
                    //AcceptSocket(socket);
                    ThreadPool.QueueUserWorkItem(AcceptSocket, socket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Thread.Sleep(100);
                }
            }
        }

        private void AcceptSocket(object state)
        {
            if (IsRuning)
            {
                Socket socket = state as Socket;
                HttpProcessor processor = new HttpProcessor(_host, socket);
                processor.ProcessRequest();
            }
        }

        public void Stop()
        {
            IsRuning = false;
            if (_serverSocket != null)
                _serverSocket.Close();
        }
    }
}
