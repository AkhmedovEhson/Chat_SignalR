using ChatService.Domain.Entities;
using ChatService.Domain.Models;
using ChatService.Hubs;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
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

builder.Services.AddDbContext<ApplicationDbContext>(ops =>
{
    ops.UseSqlServer("Server=(localdb)\\Local;Database=ShopCleanDb");
});

builder.Services.AddSingleton<IDictionary<string, string>>(o => new Dictionary<string,string>());
var app = builder.Build();
app.UseRouting();
app.UseCors();
app.MapGet("/", () => "Hello World!");
app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<ChatHub>("/chat");
});
app.Run();
