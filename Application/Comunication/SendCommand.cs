using ChatService.Domain.Common;
using ChatService.Domain.Models;
using Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comunication;
public class SendCommand:IRequest
{
    public UserConnection? Connection { get; set; }
    public string? Message { get; set; }

    public SendCommand (Context context)
    {
        HubContext = context;
    }
    internal Context HubContext { get; set; }

}

public class SendCommandHandler : IRequestHandler<SendCommand>
{
    public async Task Handle(SendCommand command,CancellationToken cancellationToken) => 
        await command.HubContext.IHubContext.Clients.Group(command.Connection?.RoomId.ToString())
                    .SendAsync(HubMethods.ReceiveMessage,command.Connection?.UserId,command.Message);

    
    
}