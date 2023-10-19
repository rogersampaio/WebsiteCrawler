using WebsiteCrawler.Interfaces;

namespace WebsiteCrawler.Commands
{
    public class ThreadManagement(
        IParsePage parsePage,
        IExtractURLs extractURLs,
        IFileManagement fileManagement
        ) : IThreadManagement
    {
        private readonly IParsePage _parsePage = parsePage;
        private readonly IExtractURLs _extractURLs = extractURLs;
        private readonly IFileManagement _fileManagement = fileManagement;

        public async void Execute(string? url, string? output)
        {
            Task.WaitAll(ExecuteAllAsync(url, output));
        }

        public async Task<List<Task>> ExecuteAllAsync(string? url, string? output)
        {
            List<Task> taskList = [];

            //1 - Get the inner content of requested URL
            string text = await _parsePage.ExecuteAsync(url);

            //2 - Save local file passing text body content or download file if it's image/svg/etc
            bool fileSaved = _fileManagement.Save(text, url, output);

            //3 - Extract new URLs from body content if it's readable and it was saved
            if (fileSaved
                && _fileManagement.IsReadable(url))
            {
                List<string>? newURLList = _extractURLs.Execute(text);
                //4 - Loop through new URLs and repeat same action
                for (int i = 0; i < newURLList?.Count; i++)
                {
                    //5 - Call same function recursively, passing URL and URL folder to create/update
                    string newUrl = newURLList[i];

                    taskList.AddRange((IEnumerable<Task>)ExecuteAllAsync(_fileManagement.GetNewUrl(url, newUrl), output));
                }
            }

            return taskList;
        }
    }
}