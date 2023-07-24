using ChatService.Domain.Common;
using ChatService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.RegularExpressions;
using ChatService.Hubs;
using ChatService.MediatR.Chat.Notifications;
using ChatService.Persistence;

namespace ChatService.MediatR.Chat.Commands
{
    public class ChatHubCommand : IRequest
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public ChatHubCommand(HubCallerContext context)
            => _context = context;

        public readonly HubCallerContext _context;
    }

    public class ChatHubCommandHandler : IRequestHandler<ChatHubCommand>
    {
        private readonly IHubContext<ChatHub> _context;
        private readonly IPublisher _publisher;
        private readonly ApplicationDbContext _dbContext;

        public ChatHubCommandHandler(IHubContext<ChatHub> context, IPublisher publisher,ApplicationDbContext dbContext)
        {
            _context = context;
            _publisher = publisher;
            _dbContext = dbContext;
        }
        public async Task Handle(ChatHubCommand userConnection, CancellationToken cancellationToken)
        {
            var room = _dbContext.Rooms.FirstOrDefault(room => room.Id == userConnection.RoomId);
            var user = _dbContext.Users.FirstOrDefault(user => user.Id == userConnection.UserId);


            if (room is null)
                throw new Exception("Room not found");



            if (user is null)
                throw new Exception("User not found");
            // Add to group.
            await _context.Groups.AddToGroupAsync(userConnection._context.ConnectionId,
                                         room.Name);


            var Event = new ChatHubConnectedEvent(_context)
                            { user = user, room = room };

            await _publisher.Publish(Event);

            

        }
    }
}
