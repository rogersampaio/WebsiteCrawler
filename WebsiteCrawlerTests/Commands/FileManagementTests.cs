using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebsiteCrawler.Commands.Tests
{

    [TestClass()]
    public class FileManagementTests
    {
        public required ILogger<FileManagement> _logger;
        public string? _output = "c:/source";

        [TestInitialize()]
        public void Initialize()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            _logger = nullLoggerFactory.CreateLogger<FileManagement>();
        }

        [TestMethod()]
        public void GetPathWithRootFolderTest()
        {
            //Given
            string? url = "https://books.toscrape.com/";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetPath(url, _output);

            //Then
            Assert.AreEqual(result, $"{_output}/");
        }

        [TestMethod()]
        public void GetPathWithChildrenFolderTest()
        {
            //Given
            string url = "https://books.toscrape.com/test";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetPath(url, _output);

            //Then
            Assert.AreEqual(result, $"{_output}/test");
        }

        [TestMethod()]
        public void GetPathWithChildrenFolderAndFileTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/test.css";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetPath(url, _output);

            //Then
            Assert.AreEqual(result, $"{_output}/test");
        }

        [TestMethod()]
        public void GetFileNameWithRootFolderTest()
        {
            //Given
            string url = "https://books.toscrape.com/";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetFilename(url);

            //Then
            Assert.AreEqual(result, "index.html");
        }

        [TestMethod()]
        public void GetFileNameWithChildrenFolderTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/test.txt";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetFilename(url);

            //Then
            Assert.AreEqual(result, "test.txt");
        }

        [TestMethod()]
        public void GetFileNameWithMultipleChildrenFolderTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/test/test/test/test.txt";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetFilename(url);

            //Then
            Assert.AreEqual(result, "test.txt");
        }

        [TestMethod()]
        public void GetFileNameWithQueryStringTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/index.html%20title=A%20la%20Mode:%20120%20Recipes%20in%2060%20Pairings:%20Pies,%20Tarts,%20Cakes";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetFilename(url);

            //Then
            Assert.AreEqual(result, "index.html");
        }

        [TestMethod()]
        public void GetFileNameWithWithStrangeNameTest()
        {
            //Given
            string url = "https://books.toscrape.com/media/cache/2c/da/2cdad67c44b002e7ead0cc35693c0e8b.jpg any new Parameter here";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetFilename(url);

            //Then
            Assert.AreEqual(result, "2cdad67c44b002e7ead0cc35693c0e8b.jpg");
        }

        [TestMethod()]
        public void SaveRootIndexTest()
        {
            //Given
            string url = "https://books.toscrape.com/";

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.SaveAsync("test", url, _output);

            //Then
            string path = fileManagement.GetPath(url, _output);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod()]
        public void SaveRootFileTest()
        {
            //Given
            string url = "https://books.toscrape.com/file.txt";

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.SaveAsync("test", url, _output);

            //Then
            string path = fileManagement.GetPath(url, _output);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod()]
        public void SaveFolderIndexTest()
        {
            //Given
            string url = "https://books.toscrape.com/test";

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.SaveAsync("test", url, _output);

            //Then
            string path = fileManagement.GetPath(url, _output);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod()]
        public void SaveFolderFileTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/file.txt";

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.SaveAsync("test", url, _output);

            //Then
            string path = fileManagement.GetPath(url, _output);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod()]
        public void SaveFontTest()
        {
            //Given
            string url = "https://books.toscrape.com/static/oscar/fonts/fontawesome-webfont.woff%3Fv=3.2.1";

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.SaveAsync("test", url, _output);

            //Then
            string path = fileManagement.GetPath(url, _output);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);

            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod()]
        public void SaveImageTest()
        {
            //Given
            string url = "https://books.toscrape.com/media/cache/2c/da/2cdad67c44b002e7ead0cc35693c0e8b.jpg";

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.SaveAsync("test", url, _output);

            //Then
            string path = fileManagement.GetPath(url, _output);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }


        [TestMethod()]
        public void SaveImageWithStrangeNameTest()
        {
            //Given
            string url = "https://books.toscrape.com/media/cache/2c/da/2cdad67c44b002e7ead0cc35693c0e8b.jpg any new Parameter here";

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.SaveAsync("test", url, _output);

            //Then
            string path = fileManagement.GetPath(url, _output);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod()]
        public void IsReadableTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/file.html";

            //When
            FileManagement fileManagement = new(_logger);
            bool result = fileManagement.IsReadable(url);

            //Then
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsReadableNotTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/file.txt";

            //When
            FileManagement fileManagement = new(_logger);
            bool result = fileManagement.IsReadable(url);

            //Then
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetNewUrlTest()
        {
            //Given
            string root = "https://books.toscrape.com/static/oscar/css/styles.css";
            string newPath = "../fonts/glyphicons-halflings-regular.eot";

            //When
            FileManagement fileManagement = new(_logger);
            string result = fileManagement.GetNewUrl(root, newPath);

            //Then
            Assert.AreEqual(result, "https://books.toscrape.com/static/oscar/fonts/glyphicons-halflings-regular.eot");
        }

        [TestMethod()]
        public void GetNewUrl2Test()
        {
            //Given
            string root = "https://books.toscrape.com";
            string newPath = "static/oscar/favicon.ico";

            //When
            FileManagement fileManagement = new(_logger);
            string result = fileManagement.GetNewUrl(root, newPath);

            //Then
            Assert.AreEqual(result, "https://books.toscrape.com/static/oscar/favicon.ico");
        }
    }
}