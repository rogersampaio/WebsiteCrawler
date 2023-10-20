using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebsiteCrawler.Commands.Tests
{
    [TestClass()]
    public class ParsePageTests
    {
        public required ILogger<ParsePage> _logger;
        public required IHttpClientFactory _clientFactory;

        [TestInitialize()]
        public void Initialize()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            _logger = nullLoggerFactory.CreateLogger<ParsePage>();
            Mock<IHttpClientFactory> clientFactoryMock = new();
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://www.google.com/")
            };
            clientFactoryMock.Setup(_ => _.CreateClient("HttpClient")).Returns(httpClient);

            _clientFactory = clientFactoryMock.Object;
        }

        [TestMethod()]
        public async Task ExecuteTest()
        {
            //Given
            string url = "https://books.toscrape.com/";
            string result;

            //When
            ParsePage parsePage = new(_logger, _clientFactory);
            result = await parsePage.ExecuteAsync(url);

            //Then
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public async Task ExecuteNullTest()
        {
            //Given
            string url = "";
            string result;

            //When
            ParsePage parsePage = new(_logger, _clientFactory);
            result = await parsePage.ExecuteAsync(url);

            //Then
            Assert.IsTrue(result != null);
        }
    }
}