using WebsiteCrawler.Controllers;
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
        public void GetPathWithRootFolderTest()
        {
            //Given
            string url = "https://books.toscrape.com/";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetPath(url);

            //Then
            Assert.AreEqual(result, "source/");
        }

        [TestMethod()]
        public void GetPathWithChildrenFolderTest()
        {
            //Given
            string url = "https://books.toscrape.com/test";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetPath(url);

            //Then
            Assert.AreEqual(result, "source/test");
        }

        [TestMethod()]
        public void GetPathWithChildrenFolderAndFileTest()
        {
            //Given
            string url = "https://books.toscrape.com/test/test.css";
            string result;

            //When
            FileManagement fileManagement = new(_logger);
            result = fileManagement.GetPath(url);

            //Then
            Assert.AreEqual(result, "source/test");
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
        public void SaveRootIndexTest()
        {
            //Given
            string url = "https://books.toscrape.com/";
            bool result;

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.Save("test", url);

            //Then
            string path = fileManagement.GetPath(url);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod()]
        public void SaveRootFileTest()
        {
            //Given
            string url = "https://books.toscrape.com/file.txt";
            bool result;

            //When
            FileManagement fileManagement = new(_logger);
            fileManagement.Save("test", url);

            //Then
            string path = fileManagement.GetPath(url);
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
            fileManagement.Save("test", url);

            //Then
            string path = fileManagement.GetPath(url);
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
            fileManagement.Save("test", url);

            //Then
            string path = fileManagement.GetPath(url);
            string fileName = fileManagement.GetFilename(url);
            string filePath = Path.Combine(path, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }
    }
}