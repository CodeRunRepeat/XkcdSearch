using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XkcdSearch.DataExtractionFunctions;

namespace DataExtractionFunctionsTest
{
    [TestClass]
    public class GetLatestComicTest
    {
        [TestMethod]
        public void TestDefault()
        {
            var request = new MockHttpRequest();
            var result = GetLatestComicFunction.Run(request, new DebugLogger("GetLatestComicTest"));
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result.Result).Value);

            string value = ((OkObjectResult)result.Result).Value.ToString();
            int output;
            Assert.IsTrue(int.TryParse(value, out output));
        }
    }
}
