using Application;
using ChatService;
using ChatService.Common.Interfaces;
using ChatService.Domain.Entities;
using ChatService.Domain.Models;
using ChatService.Filters;
using ChatService.Hubs;
using Domain;
using Domain.Common.Dispatcher;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddUIServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(p =>
    {
        p.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


builder.Services.AddSingleton<IDictionary<string, string>>(o => new Dictionary<string,string>());
var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors();
app.MapGet("/", () => "Hello World!");
app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<ChatHub>("/chat");
});
app.Run();
