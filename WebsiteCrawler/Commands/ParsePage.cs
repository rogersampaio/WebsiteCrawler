using System.Net;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Commands
{
    public class ParsePage(ILogger<ParsePage> logger, IHttpClientFactory clientFactory) : IParsePage
    {
        private readonly ILogger _logger = logger;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

        public async Task<string> ExecuteAsync(string? url)
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
                var httpClient = _clientFactory.CreateClient("HttpClient");
                var response = await httpClient.GetAsync(url);
                var streamResponse = await response.Content.ReadAsStringAsync();
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return streamResponse;
                }
                else
                {
                    throw new Exception("ParsePage error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ParsePage error: {url}, message: {Message}", url, ex.Message);
                throw;
            }

        }

        
    }


}
