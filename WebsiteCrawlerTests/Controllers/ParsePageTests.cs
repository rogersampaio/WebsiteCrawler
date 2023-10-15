using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebsiteCrawler.Controllers.Tests
{
    [TestClass()]
    public class ParsePageTests
    {

        public required ILogger<ParsePage> _logger;

        [TestInitialize()]
        public void Initialize()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            _logger = nullLoggerFactory.CreateLogger<ParsePage>();
        }

        [TestMethod()]
        public async Task ExecuteTest()
        {
            //Given
            string url = "https://books.toscrape.com/";
            string result;

            //When
            ParsePage parsePage = new(_logger);
            result = await parsePage.Execute(url);

            //Then
            Assert.IsTrue(result != null);
        }
    }
}