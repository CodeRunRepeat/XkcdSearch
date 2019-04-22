using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace XkcdSearch.DataExtractionFunctions
{
    public static class GetComicInformationFunction
    {
        [FunctionName("GetComicInformation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string comicId = await ParameterManager.GetParameterValue(req, "comicId", (d) => d?.comicId);
                comicId = ParameterManager.SanitizeParameter(comicId, ParameterManager.ParameterType.Numeric, "");

                XkcdDataCollector collector = new XkcdDataCollector();
                var result = await collector.GetComicInformation(comicId);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "GetComicInformation failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
