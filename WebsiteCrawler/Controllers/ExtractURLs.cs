using System.Text.RegularExpressions;

namespace WebsiteCrawler.Controllers
{
    public partial class ExtractURLs : IExtractURLs
    {
        public List<string>? Execute(string? text)
        {
            if (text == null)
                return null;

            List<string>? list = [];
            Regex urlRx = MyRegex();
            //(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?

            MatchCollection matches = urlRx.Matches(text);
            foreach (Match match in matches.Cast<Match>())
            {
                list.Add(match.Value);
            }
            return list;
        }

        [GeneratedRegex(@"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex MyRegex();
    }
}
