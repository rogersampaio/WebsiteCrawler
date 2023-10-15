using Microsoft.AspNetCore.Mvc;

namespace WebsiteCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebCrawlerController(
        ILogger<WebCrawlerController> logger,
        IParsePage parsePage) : ControllerBase
    {
        private readonly ILogger<WebCrawlerController> _logger = logger;
        private readonly IParsePage _parsePage = parsePage;
        private readonly IExtractURLs _extractURLs;

        [HttpPost(Name = "PostStartCrawling")]
        public async Task<bool> PostAsync(Request request)
        {
            ArgumentNullException.ThrowIfNull(request?.Url);

            DateTime startTime = DateTime.Now;
            _logger.LogInformation($"Starting crawling {request?.Url} at {startTime}");

            //Get the inner content of requested URL
            string mainBody = await _parsePage.Execute(request?.Url);

            //Extract new URLs from body content if it's HTML
            List<string>? newURLList = _extractURLs.Execute(mainBody);

            //Save local file with body content

            //Loop through new URLs and repeat same action
            for (int i = 0; i < newURLList?.Count; i++)
            {
                string url = newURLList[i];
                //Call same function recursively, passing URL and URL folder to create/update
            }

            _logger.LogInformation($"Finished crawling in {(DateTime.Now - startTime).TotalSeconds} seconds");
            return false;
        }
    }
}
