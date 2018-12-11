using System;
using System.Web;

namespace LyoServer
{
    public class LyoHost : MarshalByRefObject
    {
        public string PhysicalDir { get; private set; }

        public string VituralDir { get; private set; }
        
        public void Config(string vitrualDir, string physicalDir)
        {
            VituralDir = vitrualDir;
            PhysicalDir = physicalDir;
        }

        /// <summary>
        /// 动态网页处理
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="requestInfo"></param>
        public void ProcessRequest(HttpProcessor processor, RequestInfo requestInfo)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(string.Format("{0} {1} from {2}", requestInfo.HttpMethod, requestInfo.RawUrl, requestInfo.RemoteEndPoint));

            WorkerRequest workerRequest = new WorkerRequest(this, processor, requestInfo);
            HttpRuntime.ProcessRequest(workerRequest);
        }
    }
}
