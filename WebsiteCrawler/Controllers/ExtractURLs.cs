using System.Text.RegularExpressions;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Controllers
{
    public partial class ExtractURLs(ILogger<ExtractURLs> logger) : IExtractURLs
    {
        private readonly ILogger _logger = logger;

        public List<string>? Execute(string? text)
        {
            if (text == null)
                return null;

            List<string>? list = [];
            Regex hrefRegex = HrefRegex();

            MatchCollection matches = hrefRegex.Matches(text);
            foreach (Match match in matches.Cast<Match>())
            {
                list.Add(match.Value.Replace("href=", "").Replace("\"", ""));
            }

            List<string> distinctList = list.Distinct().ToList();
            _logger.LogInformation($"Extracted {distinctList.Count} URLs from body");
            return distinctList;
        }

        [GeneratedRegex($"href=\"(.*)\"", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex HrefRegex();
    }
}
