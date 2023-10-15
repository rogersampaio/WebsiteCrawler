using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    public class ParsePage(ILogger<ParsePage> logger) : IParsePage
    {
        private readonly ILogger _logger = logger;

        public async Task<string> Execute(string? url)
        {
            if (String.IsNullOrEmpty(url))
                return "";

            _logger.LogInformation("Getting source code of {url}", url);
            var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var response = await myClient.GetAsync(url);
            var streamResponse = await response.Content.ReadAsStringAsync();
            return streamResponse;
        }
    }
}
