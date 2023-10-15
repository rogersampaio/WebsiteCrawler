using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebsiteCrawler.Controllers.Tests
{

    [TestClass()]
    public class FileManagementTests
    {
        public required ILogger<FileManagement> _logger;

        [TestInitialize()]
        public void Initialize()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            _logger = nullLoggerFactory.CreateLogger<FileManagement>();
        }

        [TestMethod()]
        public void SaveRootTest()
        {
            //Given
            string url = "https://books.toscrape.com/";
            bool result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.Save("test", url);

            //Then
            Assert.IsFalse(result);
        }
    }
}