using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace XkcdSearch.DataExtractionFunctions
{
    static class ParameterManager
    {
        public enum ParameterType
        {
            Numeric
        }

        public static async Task<string> GetParameterValue(HttpRequest req, string parameterName, Func<dynamic, string> propertyGetter)
        {
            string value = req.Query[parameterName];
            if (!string.IsNullOrEmpty(value))
                return value;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            return propertyGetter(data);
        }

        public static string SanitizeParameter(string value, ParameterType expectedType, string defaultValue)
        {
            switch (expectedType)
            {
                case ParameterType.Numeric:
                    long numericValue;
                    if (long.TryParse(value, out numericValue))
                        return value;
                    break;
            }

            return defaultValue;
        }
    }
}
