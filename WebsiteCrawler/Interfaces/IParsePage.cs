namespace WebsiteCrawler.Interfaces
{
    public interface IParsePage
    {
        public Task<string> Execute(string? url);
    }
}
