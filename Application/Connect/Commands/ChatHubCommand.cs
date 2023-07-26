
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using ChatService.MediatR.Chat.Notifications;
using Microsoft.EntityFrameworkCore;
using Domain;
using Application.Common.Interfaces;
using ChatService.Domain.Models;

namespace Application.Connect.Commands
{
    public class ChatHubCommand : IRequest
    {

        public UserConnection? Connection { get; set; }
        public ChatHubCommand(Context hubContext)
            => HubContext = hubContext;

        internal Context HubContext { get; set; }
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
        public async Task Handle(ChatHubCommand command, CancellationToken cancellationToken)
        {

            if (command.Connection is null)
                throw new Exception("User send empty connection");
            var room = _dbContext.Rooms
                    .Include(x => x.Listeners)
                    .Where(x => x.Id == command.Connection.RoomId)
                    .FirstOrDefault()
                    ?? throw new Exception("Room does not exist !");




            var user = await _dbContext.Users
                .Include(x => x.Rooms)
                .Where(user => user.Id == command.Connection.UserId)
                .FirstOrDefaultAsync()
                ?? throw new Exception("User not found");



            if (!room.Listeners.Contains(user))
                throw new Exception("You aren't part of the room");



            // Add to group.
            await command.HubContext.IHubContext.Groups.AddToGroupAsync(command.HubContext.HubCallerContext.ConnectionId,
                                         room.Id.ToString());


            var Event = new ChatHubConnectedEvent(command.HubContext.IHubContext)
            {
                user = user,
                room = room,
            };

            await _publisher.Publish(Event);




        }
    }
}
