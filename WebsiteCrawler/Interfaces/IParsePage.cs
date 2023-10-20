namespace WebsiteCrawler.Interfaces
{
    public interface IParsePage
    {
        public Task<string> ExecuteAsync(string? url);
    }
}
