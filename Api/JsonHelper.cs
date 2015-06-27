using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Api
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings GetJsonSettings(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            return jsonFormatter.SerializerSettings;
        }

        /// <summary>
        /// Ensures our web api endpoints return json with properties that are in
        /// camel case, as opposed to proper casing, so that the client-side js code
        /// works as expected.
        /// </summary>
        public static void RegisterCamelCaseJsonFormatter(this HttpConfiguration config)
        {
            var settings = GetJsonSettings(config);
            settings.Formatting = Formatting.None;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
