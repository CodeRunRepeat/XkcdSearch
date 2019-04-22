using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XkcdSearch.DataExtractionFunctions;

namespace DataExtractionFunctionsTest
{
    [TestClass]
    public class GetComicUrlTest
    {
        [TestMethod]
        public void TestExisting()
        {
            var request = new MockHttpRequest();
            request.AddQueryParameter("comicId", "123");
            var result = GetComicUrlFunction.Run(request, new DebugLogger("GetComicUrlTest"));
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);
            Assert.IsTrue(((OkObjectResult)result.Result).Value.ToString().StartsWith("https://"));
        }
    }
}