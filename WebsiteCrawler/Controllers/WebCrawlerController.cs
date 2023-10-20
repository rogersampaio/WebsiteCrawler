using Microsoft.AspNetCore.Mvc;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebCrawlerController(
        ILogger<WebCrawlerController> logger,
        IThreadManagement threadManagement) : ControllerBase
    {
        private readonly ILogger<WebCrawlerController> _logger = logger;
        private readonly IThreadManagement _threadManagement = threadManagement;

        [HttpPost(Name = "PostStartCrawling")]
        public string Post(Request request)
        {
            DateTime startTime = DateTime.Now;
            _threadManagement.Execute(request.Url, request.Output);

            string result = $"PostStartCrawling finished successfully in: {(DateTime.Now - startTime).TotalSeconds} seconds";
            _logger.LogInformation(result);
            return result;
        }
    }
}
