using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace LyoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port;
            string dir = Directory.GetCurrentDirectory();
            if (args.Length == 0 || !int.TryParse(args[0], out port))
            {
                port = int.Parse(Config.CreatInstance().GetConfigValue("port"));
            }

            InitHostFile(dir);
            LyoHost host = (LyoHost)ApplicationHost.CreateApplicationHost(typeof(LyoHost), "/", dir);
            host.Config("/", dir);

            WebServer server = new WebServer(host, port);
            server.Start();
        }

        //需要拷贝执行文件 才能创建ASP.NET应用程序域
        private static void InitHostFile(string dir)
        {
            string path = Path.Combine(dir, "bin");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string source = Assembly.GetExecutingAssembly().Location;
            string target = path + "/" + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            if (File.Exists(target))
                File.Delete(target);
            File.Copy(source, target);
        }
    }
}
