namespace WebsiteCrawler.Interfaces
{
    public interface IFileManagement
    {
        bool Save(string? text, string? url, string? output);
        bool IsReadable(string? url);
        string GetNewUrl(string? root, string newPath);
    }
}
