using WebsiteCrawler.Commands;
using WebsiteCrawler.Controllers;
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
