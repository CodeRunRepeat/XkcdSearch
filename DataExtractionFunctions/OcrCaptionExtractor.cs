using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Linq;
using System.Threading.Tasks;

namespace XkcdSearch.DataExtractionFunctions
{
    class OcrCaptionExtractor
    {
        private ComputerVisionClient client;
        private const char separator = ' ';
        public OcrCaptionExtractor(string endpoint, string apiKey)
        {
            client = new ComputerVisionClient(
                            new ApiKeyServiceClientCredentials(apiKey),
                            new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = endpoint
            };
        }

        public async Task<string> GetTextFromImage(string imageUrl)
        {
            var textHeaders = await client.BatchReadFileAsync(imageUrl, TextRecognitionMode.Handwritten);
            return await GetOcrResults(textHeaders.OperationLocation);
        }

        private async Task<string> GetOcrResults(string operationLocation)
        {
            string operationId = operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);

            ReadOperationResult result = await client.GetReadOperationResultAsync(operationId);

            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;
            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                await Task.Delay(1000);
                result = await client.GetReadOperationResultAsync(operationId);
            }

            return
                string.Join(
                    separator,
                    result.RecognitionResults.Select(
                        r => string.Join(
                            separator, 
                            r.Lines.Select(l => l.Text).ToArray()))
                    .ToArray());
            
        }
    }
}
