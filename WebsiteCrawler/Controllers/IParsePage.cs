namespace WebsiteCrawler.Controllers
{
    public interface IParsePage
    {
        Task<string> Execute(string? url);
    }
}
