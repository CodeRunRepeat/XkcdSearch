using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XkcdSearch.DataExtractionFunctions;

namespace DataExtractionFunctionsTest
{
    [TestClass]
    public class GetComicInformationTest
    {
        [TestMethod]
        public void TestExisting()
        {
            var request = new MockHttpRequest();
            request.AddQueryParameter("comicId", "123");
            var result = GetComicInformationFunction.Run(request, new DebugLogger("GetComicInformationTest"));
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);
            Assert.IsInstanceOfType(((OkObjectResult)result.Result).Value, typeof(XkcdComicInformation));
            Assert.IsTrue(((XkcdComicInformation)((OkObjectResult)result.Result).Value).ComicUrl.StartsWith("https://"));
        }

        [TestMethod]
        public void TestSpecialChars()
        {
            var request = new MockHttpRequest();
            request.AddQueryParameter("comicId", "1");
            var result = GetComicInformationFunction.Run(request, new DebugLogger("GetComicInformationTest"));
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);
            Assert.IsInstanceOfType(((OkObjectResult)result.Result).Value, typeof(XkcdComicInformation));
            Assert.IsTrue(((XkcdComicInformation)((OkObjectResult)result.Result).Value).ComicUrl.Contains("(1)"));
        }
    }
}