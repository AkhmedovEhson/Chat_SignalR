

using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application
{
    public class Command:IRequest<string>
    {
        public readonly Hub context;
        public Command(Hub context) 
            => this.context = context;
    }

    public class CommandHandler:IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command command,CancellationToken cancellationToken)
        {

            await command.context.Clients.All.SendAsync("all", "Command Occured");
            
            return await Task.FromResult(string.Empty);
        }

    }
}