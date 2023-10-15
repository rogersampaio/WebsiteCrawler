using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    public class FileManagement : IFileManagement
    {
        private readonly ILogger _logger;

        public FileManagement(ILogger<FileManagement> logger)
        {
            _logger = logger;
        }

        public bool Save(string? text, string? url)
        {
            _logger.LogInformation($"saving file {url}");


            throw new NotImplementedException();
        }
    }
}