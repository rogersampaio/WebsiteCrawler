using Microsoft.Extensions.Logging;
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
            string text = "<link rel=\"stylesheet\" href=\"static/oscar/css/styles.css\" type=\"text/css\"  />\r\n    " +
                "\r\n    <link rel=\"stylesheet\" href=\"static/oscar/css/styles.css\" />";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 1);
        }

        [TestMethod()]
        public void ExecuteImageTest()
        {
            //Given
            string text = "<img src=\"media/cache/2c/da/2cdad67c44b002e7ead0cc35693c0e8b.jpg\" alt=\"A Light in the Attic\" class=\"thumbnail\">";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 1);
        }

        [TestMethod()]
        public void ExecuteScriptTest()
        {
            //Given
            string text = "<script src=\"static/oscar/js/bootstrap-datetimepicker/bootstrap-datetimepicker.js\" type=\"text/javascript\" charset=\"utf-8\"></script>";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 1);
        }

        [TestMethod()]
        public void ExecuteFontsTest()
        {
            //Given
            string text = "@font-face {\r\n  font-family: 'Glyphicons Halflings';\r\n  " +
                "src: url('../fonts/glyphicons-halflings-regular.eot');\r\n  " +
                "src: url('../fonts/glyphicons-halflings-regular.eot%3F') format('embedded-opentype'), " +
                "url('../fonts/glyphicons-halflings-regular.woff') format('woff'), " +
                "url('../fonts/glyphicons-halflings-regular.ttf') format('truetype'), " +
                "url('../fonts/glyphicons-halflings-regular.svg') format('svg');\r\n}";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 5);
        }

        [TestMethod()]
        public void ExecuteFonts2Test()
        {
            //Given
            string text = "src: url('../fonts/fontawesome-webfont.eot%3Fv=3.2.1');\r\n  " +
                "src: url('../fonts/fontawesome-webfont.eot%3F') format('embedded-opentype'), " +
                "url('../fonts/fontawesome-webfont.woff%3Fv=3.2.1') format('woff'), " +
                "url('../fonts/fontawesome-webfont.ttf%3Fv=3.2.1') format('truetype'), " +
                "url('../fonts/fontawesome-webfont.svg') format('svg');";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 5);
        }

        [TestMethod()]
        public void ExecuteJqueryTest()
        {
            //Given
            string text = "  <!-- jQuery -->\r\n            <script src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js\"></script>\r\n            " +
                "<script>window.jQuery || document.write('<script src=\"../../../../static/oscar/js/jquery/jquery-1.9.1.min.js\"><\\/script>')</script>";
            List<string>? urlList = [];

            //When
            ExtractURLs extractURLs = new(_logger);
            urlList = extractURLs.Execute(text);

            //Then
            Assert.IsTrue(urlList?.Count == 1);
        }

    }
}