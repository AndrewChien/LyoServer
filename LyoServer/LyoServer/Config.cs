using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LyoServer
{
    public class Config
    {
        public static string ConfigPath = "config";

        private static Dictionary<string, string> configs;
        public static Dictionary<string, string> Configs
        {
            get { return configs; }
        }

        private Config()
        {
            ReadConfig();
        }

        static Config instance = null;
        public static Config CreatInstance()
        {
            if(instance==null)
            {
                instance = new Config();
            }
            return instance;
        }

        private static void ReadConfig()
        {
            var contentDictionary = new Dictionary<string, string>();
            if (!File.Exists(ConfigPath))
            {
                return;
            }
            FileStream fileStream = null;
            StreamReader streamReader = null;
            try
            {
                fileStream = new FileStream(ConfigPath, FileMode.Open, FileAccess.Read);
                streamReader = new StreamReader(fileStream, Encoding.Default);
                fileStream.Seek(0, SeekOrigin.Begin);
                string content = streamReader.ReadLine();
                while (content != null)
                {
                    if (content.Contains("="))
                    {
                        string key = content.Substring(0, content.LastIndexOf("=")).Trim();
                        string value = content.Substring(content.LastIndexOf("=") + 1).Trim();
                        if (!contentDictionary.ContainsKey(key))
                        {
                            contentDictionary.Add(key, value);
                        }
                    }
                    content = streamReader.ReadLine();
                }
            }
            catch
            {
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            configs = contentDictionary;
        }

        public string GetConfigValue(string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                return "";
            }
            else
            {
                return configs[key];
            }
        }
    }
}
