namespace WebsiteCrawler.Controllers
{
    public interface IExtractURLs
    {
        List<string>? Execute(string? text);
    }
}
