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
        public async Task Post(Request request)
        {
            ArgumentNullException.ThrowIfNull(request?.Url);

            DateTime startTime = DateTime.Now;
            _logger.LogInformation("Starting crawling {Url} at {startTime}", request?.Url, startTime);

            //1 - Get the inner content of requested URL
            string mainBody = await _parsePage.Execute(request?.Url);

            //2 - Save local file passing text body content
            bool fileSaved = _fileManagement.Save(mainBody, request?.Url);

            //3 - Extract new URLs from body content if it's HTML and it was saved
            if (fileSaved && _fileManagement.IsHtml(request?.Url))
            {
                List<string>? newURLList = _extractURLs.Execute(mainBody);
                //4 - Loop through new URLs and repeat same action
                Task[] taskList = [];
                for (int i = 0; i < newURLList?.Count; i++)
                {
                    //5 - Call same function recursively, passing URL and URL folder to create/update
                    string url = newURLList[i];
                    taskList.Append(Post(new Request { Url = $"{request?.Url}{url}" }));
                }
                Task.WaitAll(taskList);
            }

            _logger.LogInformation("Finished crawling in {TotalSeconds} seconds", (DateTime.Now - startTime).TotalSeconds);
        }
    }
}
