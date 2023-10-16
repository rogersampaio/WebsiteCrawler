using System.Data.SqlTypes;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    public class ParsePage(ILogger<ParsePage> logger) : IParsePage
    {
        private readonly ILogger _logger = logger;

        public async Task<string> Execute(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return "";

            if (url.Contains(".jpg", StringComparison.CurrentCultureIgnoreCase) || url.Contains(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                return "image";

            _logger.LogInformation("Getting source code of {url}", url);
            var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var response = await myClient.GetAsync(url);
            var streamResponse = await response.Content.ReadAsStringAsync();
            return streamResponse;
        }
    }
}
