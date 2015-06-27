using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Web
{
    /// <summary>
    /// Helper class for serializing data to camel cased JSON, which will be consumed by AngularJS services
    /// </summary>
    public class HtmlJsonHelper
    {
        private static JsonSerializerSettings GetJsonSettings()
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            return jsonFormatter.SerializerSettings;
        }

        /// <summary>
        /// Ensures our web api endpoints return json with properties that are in
        /// camel case, as opposed to proper casing, so that the client-side js code
        /// works as expected.
        /// </summary>
        public static void RegisterCamelCaseJsonFormatter()
        {
            var settings = GetJsonSettings();
            settings.Formatting = Formatting.None;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static string ConvertToJsonInCamelCase(object value)
        {
            return JsonConvert.SerializeObject(value, GetJsonSettings());
        }
    }
}
