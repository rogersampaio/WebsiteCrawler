﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebsiteCrawler.Controllers.Tests
{
    [TestClass()]
    public class ExtractURLsTests
    {
        public required ILogger<ExtractURLs> _logger;

        [TestInitialize()]
        public void Initialize() {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            _logger = nullLoggerFactory.CreateLogger<ExtractURLs>();
        }

        [TestMethod()]
        public void ExecuteSimpleTest()
        {
            //Given
            string text = "<link rel=\"stylesheet\" type=\"text/css\" href=\"static/oscar/css/styles.css\" />\r\n    " +
                "\r\n    <link rel=\"stylesheet\" href=\"static/oscar/js/bootstrap-datetimepicker/bootstrap-datetimepicker.css\" />\r\n    " +
                "<link rel=\"stylesheet\" type=\"text/css\" href=\"static/oscar/css/datetimepicker.css\" />";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 3);
        }

        [TestMethod()]
        public void ExecuteRemoveDuplicationTest()
        {
            //Given
            string text = "<link rel=\"stylesheet\" type=\"text/css\" href=\"static/oscar/css/styles.css\" />\r\n    " +
                "\r\n    <link rel=\"stylesheet\" href=\"static/oscar/css/styles.css\" />";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 1);
        }

        //[TestMethod()]
        //public void ExecuteImageTest()
        //{
        //    //Given
        //    string text = "<img src=\"media/cache/2c/da/2cdad67c44b002e7ead0cc35693c0e8b.jpg\" alt=\"A Light in the Attic\" class=\"thumbnail\">";
        //    List<string>? urlList = [];

        //    //When
        //    ExtractURLs extractURLs = new(_logger);
        //    urlList = extractURLs.Execute(text);

        //    //Then
        //    Assert.IsTrue(urlList?.Count == 1);
        //}
    }
}