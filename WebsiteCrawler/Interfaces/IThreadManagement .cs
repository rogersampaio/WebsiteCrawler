namespace WebsiteCrawler.Interfaces
{
    public interface IThreadManagement
    {
        void Execute(string url, string output);
        //Task<Task[]> ExecuteAsync(string? url, string? output);
    }
}
