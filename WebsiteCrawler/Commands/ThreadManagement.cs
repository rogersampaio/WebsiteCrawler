using System.Diagnostics;
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
        private readonly List<string> urlList = [];
        private readonly ManualResetEvent manualResetEvent = new(false);

        public void Execute(string url, string output)
        {
            urlList.Add(url);

            using CountdownEvent cde = new(1);

            ThreadPool.QueueUserWorkItem((state) => ExecuteAllAsync(url, output, cde));

            // ensure that at least one thread was created 
            manualResetEvent.WaitOne();

            // Decrease the counter (as it was initialized with the value 1).
            cde.Signal();

            // Wait until the counter is zero.
            cde.Wait();

            Trace.WriteLine(message: $"All threads are finished, current count: {cde.CurrentCount}");
        }

        public async Task ExecuteAllAsync(string url, string output, CountdownEvent cde)
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
                    string newUrl = _fileManagement.GetNewUrl(url, newURLList[i]);

                    if (!urlList.Contains(newUrl)) {
                        urlList.Add(newUrl);

                        // Increase the counter
                        cde.AddCount();

                        ThreadPool.QueueUserWorkItem((state) => ExecuteAllAsync(newUrl, output, cde));
                    }
                }
            }

            // Decrease the counter
            cde.Signal();

            // Signal that at least one thread is executed.
            manualResetEvent.Set();
        }
    }
}