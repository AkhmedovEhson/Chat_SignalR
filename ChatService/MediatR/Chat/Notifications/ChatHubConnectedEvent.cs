using ChatService.Domain.Common;
using ChatService.Domain.Entities;
using ChatService.Domain.Models;
using ChatService.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Domain;

namespace ChatService.MediatR.Chat.Notifications
{
    public class ChatHubConnectedEvent : BaseEvent
    {
        public User user { get; set; }
        public Room room { get; set; }
        public readonly IHubContext<ChatHub> _context;
        public ChatHubConnectedEvent(IHubContext<ChatHub> context)
        {
            _context = context;
        }
    }

    public class ChatHubConnectedEventHandler : INotificationHandler<ChatHubConnectedEvent>
    {
        public async Task Handle(ChatHubConnectedEvent Event, CancellationToken cancellationToken)
        {
            Log.Information("| - - - - ChatService messaged: Connected Event - - - - |");
            await Event._context.Clients.Group(Event.room.Name)
                           .SendAsync(HubMethods.ReceiveMessage, $"Someone with ID '{Event.user.Username}' has connected");
        }
    }

}
