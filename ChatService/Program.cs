using ChatService.Domain.Models;
using ChatService.Hubs;

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
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(o => new Dictionary<string, UserConnection>());
var app = builder.Build();
app.UseRouting();
app.UseCors();
app.MapGet("/", () => "Hello World!");
app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<ChatHub>("/chat");
});
app.Run();
