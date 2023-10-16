using System.Data.SqlTypes;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    public class ParsePage(ILogger<ParsePage> logger) : IParsePage
    {
        private readonly ILogger _logger = logger;

        public async Task<string> Execute(string? url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return "";

                if (url.Contains(".jpg", StringComparison.CurrentCultureIgnoreCase) 
                    || url.Contains(".jpeg", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".woff", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".eot", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".svg", StringComparison.CurrentCultureIgnoreCase)
                    || url.Contains(".ttf", StringComparison.CurrentCultureIgnoreCase))
                    return "specialFile";

                //_logger.LogInformation("Getting source code of {url}", url);
                var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
                var response = await myClient.GetAsync(url);
                var streamResponse = await response.Content.ReadAsStringAsync();
                return streamResponse;
            }
            catch (Exception)
            {
                _logger.LogError("ParsePage error: {url}", url);
                throw;
            }
            
        }
    }
}
