using Polly;
using Polly.Extensions.Http;
using WebsiteCrawler.Commands;
using WebsiteCrawler.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure dependencies
builder.Services.AddScoped<IParsePage, ParsePage>();
builder.Services.AddScoped<IExtractURLs, ExtractURLs>();
builder.Services.AddScoped<IFileManagement, FileManagement>();

builder.Services.AddHttpClient("HttpClient").AddPolicyHandler(GetRetryPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
      // Handle HttpRequestExceptions, 408 and 5xx status codes
      .HandleTransientHttpError()
      // Handle 404 not found
      .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
      // Handle 401 Unauthorized
      .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      // What to do if any of the above erros occur:
      // Retry 8 times, each time wait 1, 2, 3, 4, 5, 6, 7, and 8 seconds before retrying.
      .WaitAndRetryAsync(8, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
}