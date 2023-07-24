using MediatR;
using Serilog;

namespace ChatService.MediatR.Chat.Notifications
{
    public class ChatHubConnectedEvent : INotification
    { }

    public class ChatHubConnectedEventHandler : INotificationHandler<ChatHubConnectedEvent>
    {
        public Task Handle(ChatHubConnectedEvent Event,CancellationToken cancellationToken)
        {
            Log.Information("| - - - - ChatService messaged: Connected Event - - - - |");
            return Task.CompletedTask;
        }
    }

}
