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

        public static JObject GetGtsCredentials(string filePath)
        {
            return JObject.Parse(File.ReadAllText(filePath));
        }

        public static JObject ReadJsonFile(string filePath)
        {
            return JObject.Parse(File.ReadAllText(filePath));
        }

        // New method to retrieve the tracking number
        public static string GetTrackingNumber(string filePath)
        {
            var jsonContent = ReadJsonFile(filePath);

            // Navigating the JSON structure to get the tracking number
            string trackingNumber = jsonContent["shipments"]?[0]?["package"]?["trackingNumber"]?.ToString();
            return trackingNumber;
        }
    }
}
