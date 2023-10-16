using System;
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
                string url = match.Value.Replace("href=", "").Replace("\"", "");
                if (url.Contains(' '))
                    //using range operator
                    url = url[..url.IndexOf(' ')];
                list.Add(url);
            }

            Regex sourceRegex = SourceRegex();
            matches = sourceRegex.Matches(text);
            foreach (Match match in matches.Cast<Match>())
            {
                string url = match.Value.Replace("src=", "").Replace("\"", "");
                if (url.Contains("http"))
                    continue;
                if (url.Contains(' '))
                    url = url[..url.IndexOf(' ')];
                list.Add(url);
            }

            Regex urlRegex = UrlRegex();
            matches = urlRegex.Matches(text);
            foreach (Match match in matches.Cast<Match>())
            {
                string url = match.Value.Replace("url('", "").Replace("')", "");
                list.Add(url);
            }

            List<string> distinctList = list.Distinct().ToList();
            //_logger.LogInformation("Extracted {Count} URLs from body", distinctList.Count);
            return distinctList;
        }

        [GeneratedRegex($"href=\"(.*)\"", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex HrefRegex();

        [GeneratedRegex($"src=\"(.*)\"", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex SourceRegex();

        [GeneratedRegex(@"\b(?:url\(')\S+\b", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex UrlRegex();
    }
}
