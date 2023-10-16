using System.Net;
using System.Text;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    public partial class FileManagement(ILogger<FileManagement> logger) : IFileManagement
    {
        private readonly ILogger _logger = logger;

        /// <summary>
        /// Save file if if does not exist
        /// </summary>
        /// <param name="text">Content of the new file</param>
        /// <param name="url">URL of the original file</param>
        /// <returns>False if </returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Save(string? text, string? url, string? output)
        {
            if (string.IsNullOrEmpty(text))
            {
                _logger.LogInformation("File does not contains any text: {filePath}", url);
                return false;
            }

            if (string.IsNullOrEmpty(url))
            {
                _logger.LogInformation("URL is null");
                return false;
            }

            string path = GetPath(url, output);
            string fileName = GetFilename(url);
            string filePath = Path.Combine(path, fileName);

            if (File.Exists(filePath)) {
                _logger.LogInformation("File already exists: {filePath}", filePath);
                return false;
            }

            try
            {
                _logger.LogInformation("Saving file locally: {filePath}", filePath);

                Directory.CreateDirectory(path);

                if (url.Contains(".jpg", StringComparison.CurrentCultureIgnoreCase) 
                    || url.Contains(".jpeg", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".woff", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".eot", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".svg", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".ttf", StringComparison.CurrentCultureIgnoreCase))
                {
                    WebClient webClient = new();
                    webClient.DownloadFile(url, filePath);
                }
                else
                {
                    using FileStream fs = File.Create(filePath);
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
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

        public string GetPath(string? url, string? output)
        {
            if (string.IsNullOrEmpty(url))
                return "";

            Uri myUri = new(url);
            string absolutePath = myUri.AbsolutePath;
            
            //new for me: using index operator
            string fileName = myUri.Segments[^1];

            if (fileName.Contains('.')) {
                string pathWithoutFileName = absolutePath.Replace($"/{fileName}", "");
                return $"{output}{pathWithoutFileName}";
            }
            else
            {
                return $"{output}{myUri.AbsolutePath}";
            }            
        }

        public string GetFilename(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return "/";

            Uri myUri = new(url);
            //using index operator
            string fileName = myUri.Segments[^1];

            if (fileName.Contains('.'))
            {
                if (fileName.Contains(".html") && fileName.Contains('%'))
                    return fileName[..fileName.IndexOf('%')];
                else
                    return fileName;
            }
            else
            {
                return "index.html";
            }
        }

        public bool IsReadable(string? url) {
            string fileName =  GetFilename(url);
            return fileName.Contains(".html") || fileName.Contains(".css");
        }

        public string GetNewUrl(string? root, string newPath)
        {
            if (root == null)
                return "";

            string filename = GetFilename(root);
            string newRoot = root;
            if (root.Contains(filename))
                newRoot = root[..root.IndexOf(filename)];
            Uri uri = new(newRoot);
            string domain = root;
            if (uri.AbsolutePath.Length > 1)
                domain = root[..root.IndexOf(uri.AbsolutePath)];

            int removeSegments = 0;
            foreach (string segment in newPath.Split('/'))
            {
                if (segment == "..")
                    removeSegments++;
            }
            
            string result = domain;
            for (int i = 0; i < uri.Segments.Length - removeSegments; i++)
            {
                result += uri.Segments[i];
            }
            result += newPath.Replace("../", "");
            return result;
        }
    }
}