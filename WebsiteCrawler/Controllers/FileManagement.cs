using System.Text;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    public class FileManagement(ILogger<FileManagement> logger) : IFileManagement
    {
        private readonly ILogger _logger = logger;

        /// <summary>
        /// Save file if if does not exist
        /// </summary>
        /// <param name="text">Content of the new file</param>
        /// <param name="url">URL of the original file</param>
        /// <returns>False if </returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Save(string? text, string? url)
        {
            string path = GetPath(url);
            Directory.CreateDirectory(path);
            string fileName = GetFilename(url);
            string filePath = Path.Combine(path, fileName);

            if (String.IsNullOrEmpty(text)) {
                _logger.LogInformation("File does not contains any text: {filePath}", filePath);
                return false;
            }

            if (File.Exists(filePath)) {
                _logger.LogInformation("File already exists: {filePath}", filePath);
                return false;
            }

            try
            {
                _logger.LogInformation("Saving file locally: {filePath}", filePath);
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(filePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }

            catch (Exception)
            {
                _logger.LogError("Exception Saving file: {filePath}", filePath);
                throw;
            }

            return true;
        }

        public string GetPath(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return "/";

            Uri myUri = new(url);
            string absolutePath = myUri.AbsolutePath;
            
            //new for me: using index operator
            string fileName = myUri.Segments[^1];

            if (fileName.Contains('.')) {
                string pathWithoutFileName = absolutePath.Replace($"/{fileName}", "");
                return $"source{pathWithoutFileName}"; ;
            }
            else
            {
                return $"source{myUri.AbsolutePath}";
            }            
        }

        public string GetFilename(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return "/";

            Uri myUri = new(url);

            //new for me: using index operator
            string fileName = myUri.Segments[^1];

            if (fileName.Contains('.'))
            {
                return fileName;
            }
            else
            {
                return "index.html";
            }
        }

        public bool IsHtml(string? url) {
            string fileName =  GetFilename(url);
            return fileName.Contains(".html");
        }
    }
}