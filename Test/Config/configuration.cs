using System;
using System.Collections.Generic;

namespace Configuration
{
    public class EnvironmentConfig
    {
        public static string Environment { get; set; }
        public static string BaseURL { get; set; }

        public static EnvironmentConfig GetEnvironment(string environment = null)
        {
            environment = environment ?? System.Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "PMS_E2E";
            string baseURL = GetBaseURLForEnvironment(environment);

            Environment = environment;
            BaseURL = baseURL;

            return new EnvironmentConfig();
        }

        private static string GetBaseURLForEnvironment(string environment)
        {
            var urls = GetAllUrls();

            if (urls.ContainsKey(environment))
            {
                return urls[environment];
            }
            else
            {
                return "https://as-indicina-ncus-sandbox.azurewebsites.net/api/v1/Shipment/Manifest";
            }
        }

        public static Dictionary<string, string> GetAllUrls()
        {
            var urls = new Dictionary<string, string>
            {
                { "PMS_E2E", "https://as-indicina-ncus-sandbox.azurewebsites.net/api/v1/Shipment/Manifest" },
                { "MLO_E2E", "https://label-generator-service.maersk-digital.dev/api/v1/label/generate" },
                { "MSP_E2E", "https://sorting-service.maersk-digital.dev/api/container" },
                { "CUSTOM_E2E", "https://internalportal-test.b2ceurope.eu/datatable/international_customs" },
                { "MTS_E2E", "https://mec-carrier-tracking-usps-api-dev.maersk-digital.dev/api/v4.0/Tracking" },
                { "GTS_E2E", "https://wwa-globaltrackingsolution-weu-staging.azurewebsites.net/api/Tracking/ShipmentTracking" }
            };

            return urls;
        }
    }
}
