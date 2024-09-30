using System.IO;
using Newtonsoft.Json.Linq;

namespace Utils
{
    public static class FileUtils
    {
        public static JObject GetCredentials(string filePath)
        {
            return JObject.Parse(File.ReadAllText(filePath));
        }
    }
}
