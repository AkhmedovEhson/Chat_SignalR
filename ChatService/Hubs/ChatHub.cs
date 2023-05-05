using ChatService.Domain.Common;
using ChatService.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    public class ChatHub:Hub
    {
        public string botUser;
        private static IDictionary<string, UserConnection> connections;
        public ChatHub(IDictionary<string,UserConnection> _connections) 
        {
            botUser = "Bot";
            connections = _connections;
            
        }
        public async Task JoinRoom(UserConnection userConnection)
        {
            KeyValuePair<string, UserConnection> keyPair 
                    = new KeyValuePair<string, UserConnection>(userConnection.User, userConnection);

            if (connections.ContainsKey(keyPair.Key))
            {
                Console.WriteLine("Already joined!");
                return;
            }
            
            await Groups.AddToGroupAsync(userConnection.User, userConnection.Room);
            connections.Add(userConnection.User,userConnection);
            //Sends notification about who joined the room.
            await Clients.Group(userConnection.Room).SendAsync(HubMethods.RECEIVEMESSAGEMETHOD,
                botUser, $"{userConnection.User} joined to {userConnection.Room}");
        }
        public async Task SendMessage(UserConnection userConnection,string message)
        {
            if (connections.TryGetValue(userConnection.User, out var connection))
            {
                
                Console.WriteLine($"{connection.User} {message}");
            
                await Clients.Group(connection.Room).SendAsync
                    (HubMethods.SENDMESSAGEMETHOD,connection.User, message);

            }
            
        }
    }
}
