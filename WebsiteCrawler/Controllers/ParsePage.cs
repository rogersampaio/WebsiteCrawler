using System.Net;

namespace WebsiteCrawler.Controllers
{
    public class ParsePage: IParsePage
    {
        private readonly ILogger _logger;

        public ParsePage(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> Execute(string? url)
        {
            return GetSourceCode(url);
        }

        private string GetSourceCode(string url)
        {
            _logger.LogInformation($"Getting source code of {url}");
            string sourceCode = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                sourceCode = sr.ReadToEnd();
                sr.Close();
                response.Close();
                _logger.LogInformation(sourceCode);
                return sourceCode;
            }
            catch
            { sourceCode = "ERROR"; }
            return sourceCode;
        }
    }
}
