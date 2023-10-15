using Microsoft.AspNetCore.Mvc;

namespace WebsiteCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebCrawlerController : ControllerBase
    {
        private readonly ILogger<WebCrawlerController> _logger;
        private readonly IParsePage _parsePage;

        public WebCrawlerController(ILogger<WebCrawlerController> logger, IParsePage parsePage)
        {
            _logger = logger;
            _parsePage = parsePage;
        }

        [HttpPost(Name = "PostStartCrawling")]
        public async Task<bool> PostAsync(Request request)
        {
            ArgumentNullException.ThrowIfNull(request?.Url);

            DateTime startTime = DateTime.Now;
            _logger.LogInformation($"Starting crawling {request?.Url} at {startTime}");

            //Get the inner content of requested URL
            string mainBody = await _parsePage.Execute(request?.Url);

            //Extract new URLs from body content if it's HTML

            //Save local file with body content

            //Loop through new URLs and repeat same action

            _logger.LogInformation($"Finished crawling in {(DateTime.Now - startTime).TotalSeconds} seconds");
            return false;
        }
    }
}
