using Microsoft.AspNetCore.Mvc;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebCrawlerController(
        ILogger<WebCrawlerController> logger,
        IParsePage parsePage,
        IExtractURLs extractURLs,
        IFileManagement fileManagement) : ControllerBase
    {
        private readonly ILogger<WebCrawlerController> _logger = logger;
        private readonly IParsePage _parsePage = parsePage;
        private readonly IExtractURLs _extractURLs = extractURLs;
        private readonly IFileManagement _fileManagement = fileManagement;

        [HttpPost(Name = "PostStartCrawling")]
        public async Task<bool> Post(Request request)
        {
            ArgumentNullException.ThrowIfNull(request?.Url);

            DateTime startTime = DateTime.Now;
            _logger.LogInformation($"Starting crawling {request?.Url} at {startTime}");

            //1 - Get the inner content of requested URL
            string mainBody = await _parsePage.Execute(request?.Url);

            //2 - Save local file passing text body content
            bool fileAlreadySaved = _fileManagement.Save(mainBody, request?.Url);

            //3 - Extract new URLs from body content if it's HTML
            if (!fileAlreadySaved)
            {
                List<string>? newURLList = _extractURLs.Execute(mainBody);
                //4 - Loop through new URLs and repeat same action
                for (int i = 0; i < newURLList?.Count; i++)
                {
                    string url = newURLList[i];
                    //5 - Call same function recursively, passing URL and URL folder to create/update
                }
            }

            _logger.LogInformation($"Finished crawling in {(DateTime.Now - startTime).TotalSeconds} seconds");
            return false;
        }
    }
}
