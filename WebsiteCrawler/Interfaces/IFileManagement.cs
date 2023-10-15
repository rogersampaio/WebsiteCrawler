namespace WebsiteCrawler.Interfaces
{
    public interface IFileManagement
    {
        bool Save(string? text, string? url);
    }
}
