using ChatService.Domain.Models;
using Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Connect.Commands
{
    public class DisconnectCommand:IRequest
    {
        public UserConnection? Connection { get; set; }
        public DisconnectCommand(Context? hubContext)
           => HubContext = hubContext;

        internal Context? HubContext { get; set; }
    }
    public class DisconnectCommandHandler : IRequestHandler<DisconnectCommand>
    {
        public async Task Handle(DisconnectCommand command,CancellationToken cancellationToken)
        {

            await command.HubContext.IHubContext.Groups
                .RemoveFromGroupAsync(command.HubContext.HubCallerContext.ConnectionId, command.Connection.RoomId.ToString());

            await  command.HubContext.IHubContext.Clients.Group(command.Connection.RoomId.ToString()).
                SendAsync("Disconnected", $"Client with ID {command.Connection.UserId} has disconnected ! ");
        }
    }
}
