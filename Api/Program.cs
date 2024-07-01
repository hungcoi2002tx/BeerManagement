using DataLayer;
using Business;
using Microsoft.EntityFrameworkCore;
using Api.Middleware;
using Microsoft.Extensions.Logging;
using Share.Constant;
using System.Reflection;
using Share.Ultils;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = configuration.Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapperConfig();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServicesDbcontext(config)
                .AddServicesDataLayer()
                .AddServicesBusinessLayer();
builder.Services.AddSingleton<Logger>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ResponseCodeValidationMiddleware>();
app.MapControllers();

app.Run();
