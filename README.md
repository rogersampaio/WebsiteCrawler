# WebsiteCrawler

This is a .Net8 API that contains only one POST endpoint which receives one URL and an output dir as a parameter and asynchronously download all the website content including inner sub-folders.

Postman example:
![image](https://github.com/rogersampaio/WebsiteCrawler/assets/21226627/7aa1a8f3-f563-49a4-a21a-2b4d0d458fd4)


**Instalation instructions:**

Option 1: Checkout the project and Run it using Visual Studio 2022.

Option 2: Checkout the project and run the WebsiteCrawler.exe executable inside "WebsiteCrawler\publish" folder.


It's an API Service, you need to call it, for example, using Postman, Swagger or any other fetch application.


**Release notes:**

21/10/2023:
  New version is optmised by adding ThreadPool, CountdownEvent and creating a list of unique url to fetch.
  The total download time of https://books.toscrape.com was 10 minutes and now it's taking 15 seconds to download 3.213 files (82.5 MB) with a 500 Mbps internet connection.
  Now it's returning the total time in the Response body.

16/10/2023:
  The crawler is working but there's still improvements to apply.
