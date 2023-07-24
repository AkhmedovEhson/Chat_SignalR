using ChatService.Domain.Common;
using ChatService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.RegularExpressions;
using ChatService.Hubs;
using ChatService.MediatR.Chat.Notifications;

namespace ChatService.MediatR.Chat.Commands
{
    public class ChatHubCommand : IRequest
    {
        public readonly HubCallerContext _context;
        public ChatHubCommand(HubCallerContext context)
            => _context = context;

        public int UserId { get; set; }
        public int RoomId { get; set; }
    }

    public class ChatHubCommandHandler : IRequestHandler<ChatHubCommand>
    {
        private readonly IHubContext<ChatHub> _context;
        public static IDictionary<string, string> connections;
        private readonly IPublisher _publisher;

        public ChatHubCommandHandler(IHubContext<ChatHub> context, IPublisher publisher)
        {
            _context = context;
            connections = new Dictionary<string, string>();
            _publisher = publisher;
        }
        public async Task Handle(ChatHubCommand userConnection, CancellationToken cancellationToken)
        {
            var room = userConnection.RoomId.ToString();


            if (room is null)
                throw new Exception("Room not found");



            // Add to group.
            await _context.Groups.AddToGroupAsync(userConnection._context.ConnectionId,
                                         room);


            Log.Information("Connected.");
            if (!connections.ContainsKey(userConnection._context.ConnectionId))
            {
                connections.Add(userConnection._context.ConnectionId,
                                room.ToString());

                await _context.Clients.Group(room.ToString())
                            .SendAsync(HubMethods.ReceiveMessage, $"Someone with ID '{userConnection._context.ConnectionId}' has connected");

                Log.Information("Added to connections list");
            }
            await _publisher.Publish(new ChatHubConnectedEvent());
            

        }
    }
}
