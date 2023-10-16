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
            ArgumentNullException.ThrowIfNull(request);

            DateTime startTime = DateTime.Now;

            //1 - Get the inner content of requested URL
            string mainBody = await _parsePage.Execute(request?.Url);

            //2 - Save local file passing text body content or download file if it's image/svg/etc
            bool fileSaved = _fileManagement.Save(mainBody, request?.Url, request?.Output);

            //3 - Extract new URLs from body content if it's readable and it was saved
            if (fileSaved 
                && _fileManagement.IsReadable(request?.Url))
            {
                List<string>? newURLList = _extractURLs.Execute(mainBody);
                //4 - Loop through new URLs and repeat same action
                Task[] taskList = [];
                for (int i = 0; i < newURLList?.Count; i++)
                {
                    //5 - Call same function recursively, passing URL and URL folder to create/update
                    string url = newURLList[i];

                    taskList.Append(Post(new Request { 
                        Url = _fileManagement.GetNewUrl(request?.Url, url),
                        Output = request?.Output ?? ""
                    }));
                }
                Task.WaitAll(taskList);
                if (newURLList?.Count > 0)
                    _logger.LogInformation("Batch of {Count} files processed: {Url}", newURLList?.Count, request?.Url);
            }
        }
    }
}
