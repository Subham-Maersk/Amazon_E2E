using System;
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

        public static void WriteJsonFile(string filePath, JObject jsonContent)
        {
            File.WriteAllText(filePath, jsonContent.ToString());
        }

        public static string GetTrackingNumber(string filePath)
        {
            var jsonContent = ReadJsonFile(filePath);
            string trackingNumber = jsonContent["shipments"]?[0]?["package"]?["trackingNumber"]?.ToString();
            return trackingNumber;
        }

        public static JObject GetMtsData(string filePath)
        {
            return ReadJsonFile(filePath);
        }

        public static string GetMtsField(string filePath, string fieldName)
        {
            var jsonContent = GetMtsData(filePath);
            return jsonContent[fieldName]?.ToString();
        }

        public static void UpdateShipmentDetails(string filePath)
        {
            var jsonContent = ReadJsonFile(filePath);

            string lastManifestNumber = jsonContent["lastManifestNumber"]?.ToString() ?? "Testing_20240_167";
            string baseManifest = "Testing_20240_"; 
            int lastManifestIncrement = int.Parse(lastManifestNumber.Replace(baseManifest, "")); // Extract number part

            string manifestNumber = baseManifest + (++lastManifestIncrement);

            int lastIncrement = jsonContent["lastIncrement"]?.Value<int>() ?? 0;
            lastIncrement++;

            string baseFormat = "TSTAMZEU90001_";
            string incrementedValue = lastIncrement.ToString("D7"); 

            string orderNumber = baseFormat + incrementedValue;
            string trackingNumber = baseFormat + incrementedValue;
            string trackingBarCodeNumber = baseFormat + incrementedValue;

            string currentDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            var shipments = jsonContent["shipments"]?[0] as JObject;
            if (shipments != null)
            {
                shipments["orderNumber"] = orderNumber;
                shipments["package"]["trackingNumber"] = trackingNumber;
                shipments["package"]["trackingBarCodeNumber"] = trackingBarCodeNumber;
                shipments["transmissionDate"] = currentDate;
                shipments["shipDate"] = currentDate;
                shipments["deliveryDate"] = currentDate;

                jsonContent["lastManifestNumber"] = manifestNumber;
                jsonContent["lastIncrement"] = lastIncrement;
            }
            else
            {
                throw new InvalidOperationException("Failed to find 'shipments' in the JSON file.");
            }

            WriteJsonFile(filePath, jsonContent);
        }
    }
}
