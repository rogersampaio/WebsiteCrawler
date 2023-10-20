using System.Text.RegularExpressions;
using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Commands
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
                    url = url[..url.IndexOf(' ')]; //range operator
                if (url.Contains("#addreview") || url.Contains("/reviews/"))
                    continue;
                if (url.Contains("><img"))
                    url = url.Replace("><img", "");
                list.Add(url);
            }

            Regex sourceRegex = SourceRegex();
            matches = sourceRegex.Matches(text);
            foreach (Match match in matches.Cast<Match>())
            {
                string url = match.Value.Replace("src=", "").Replace("\"", "");
                if (url.Contains("http") || url.Contains("html5shim.googlecode"))
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
