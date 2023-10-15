namespace WebsiteCrawler.Interfaces
{
    public interface IExtractURLs
    {
        List<string>? Execute(string? text);
    }
}
