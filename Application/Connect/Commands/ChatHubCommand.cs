
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using ChatService.MediatR.Chat.Notifications;
using Microsoft.EntityFrameworkCore;
using Domain;
using Application.Common.Interfaces;

namespace Application.Connect.Commands
{
    public class ChatHubCommand : IRequest
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }

        public ChatHubCommand(Context hubContext)
            => HubContext = hubContext;

        public Context HubContext { get; set; }
    }



    public class ChatHubCommandHandler : IRequestHandler<ChatHubCommand>
    {

        private readonly IPublisher _publisher;
        private readonly IApplicationDbContext _dbContext;

        public ChatHubCommandHandler(IPublisher publisher, IApplicationDbContext dbContext)
        {
            _publisher = publisher;
            _dbContext = dbContext;

        }
        public async Task Handle(ChatHubCommand userConnection, CancellationToken cancellationToken)
        {

            var room = _dbContext.Rooms
                    .Include(x => x.Listeners)
                    .Where(x => x.Id == userConnection.RoomId)
                    .FirstOrDefault()
                    ?? throw new Exception("Room does not exist !");




            var user = await _dbContext.Users
                .Include(x => x.Rooms)
                .Where(user => user.Id == userConnection.UserId)
                .FirstOrDefaultAsync()
                ?? throw new Exception("User not found");



            if (!room.Listeners.Contains(user))
                throw new Exception("You aren't part of the room");



            // Add to group.
            await userConnection.HubContext.IHubContext.Groups.AddToGroupAsync(userConnection.HubContext.HubCallerContext.ConnectionId,
                                         room.Id.ToString());


            var Event = new ChatHubConnectedEvent(userConnection.HubContext.IHubContext)
            {
                user = user,
                room = room,
            };

            await _publisher.Publish(Event);




        }
    }
}
