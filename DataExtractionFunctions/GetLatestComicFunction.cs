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
    public static class GetLatestComicFunction
    {
        [FunctionName("GetLatestComic")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                XkcdDataCollector collector = new XkcdDataCollector();
                var result = await collector.GetLatestComic();

                return (ActionResult)new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "GetLatestComic failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
