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
    public static class GetComicContentsFunction
    {
        [FunctionName("GetComicContents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string comic = await ParameterManager.GetParameterValue(req, "comic", (d) => d?.comic);

                OcrCaptionExtractor extractor = new OcrCaptionExtractor(
                    System.Environment.GetEnvironmentVariable("ComputerVisionEndpoint", EnvironmentVariableTarget.Process),
                     System.Environment.GetEnvironmentVariable("ComputerVisionApiKey", EnvironmentVariableTarget.Process));

                return new OkObjectResult(await extractor.GetTextFromImage(comic));
            }
            catch (Exception ex)
            {
                log.LogError(ex, "GetComicContents failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
