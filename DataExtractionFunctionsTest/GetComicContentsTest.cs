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
            request.AddQueryParameter("comic", "https://imgs.xkcd.com/comics/centrifugal_force.png");
            var result = GetComicContentsFunction.Run(request, new DebugLogger("GetComicContentsTest"));
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);
        }

        [TestMethod]
        public void TestNoQuotes()
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
            request.AddQueryParameter("comic", "https://imgs.xkcd.com/comics/canyon_small.jpg");
            var result = GetComicContentsFunction.Run(request, new DebugLogger("GetComicContentsTest"));
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);
            Assert.IsFalse(((OkObjectResult)result.Result).Value.ToString().Contains('"'));
        }
    }
}