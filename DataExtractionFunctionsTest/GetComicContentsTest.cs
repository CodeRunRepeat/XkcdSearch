using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XkcdSearch.DataExtractionFunctions;

namespace DataExtractionFunctionsTest
{
    [TestClass]
    public class GetComicContentsTest
    {
        [TestMethod]
        public void TestExisting()
        {
            var result = GetComicContents("https://imgs.xkcd.com/comics/centrifugal_force.png");

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);
        }

        [TestMethod]
        public void TestNoQuotes()
        {
            var result = GetComicContents("https://imgs.xkcd.com/comics/canyon_small.jpg");

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);
            Assert.IsFalse(((OkObjectResult)result.Result).Value.ToString().Contains('"'));
        }

        [TestMethod]
        public void TestBigImage()
        {
            var result = GetComicContents("https://imgs.xkcd.com/comics/godel_escher_kurthalsey.jpg");

            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            Assert.AreEqual(((StatusCodeResult)result.Result).StatusCode, 500);
        }

        private static System.Threading.Tasks.Task<IActionResult> GetComicContents(string url)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("local.settings.json")
                            .Build();

            System.Environment.SetEnvironmentVariable(
                GetComicContentsFunction.EndpointVariableName,
                config[GetComicContentsFunction.EndpointVariableName],
                System.EnvironmentVariableTarget.Process);
            System.Environment.SetEnvironmentVariable(
                GetComicContentsFunction.ApiKeyVariableName,
                config[GetComicContentsFunction.ApiKeyVariableName],
                System.EnvironmentVariableTarget.Process);

            var request = new MockHttpRequest();
            request.AddQueryParameter("comic", url);
            var result = GetComicContentsFunction.Run(request, new DebugLogger("GetComicContentsTest"));
            result.Wait();
            return result;
        }
    }
}