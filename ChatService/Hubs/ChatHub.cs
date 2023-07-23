using ChatService.Domain.Common;
using ChatService.Domain.Entities;
using ChatService.Domain.Models;
using ChatService.Persistence;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ChatService.Hubs
{
    public class ChatHub:Hub
    {

        private static IDictionary<string,string> connections;


        public ChatHub(IDictionary<string,string> _connections) 
        {
            
            connections = _connections;
        }









        public async Task ConnectToRoom(UserConnection userConnection)
        {
           
            var room = userConnection.RoomId.ToString();


            if (room is null)
                throw new Exception("Room not found");

            

            // Add to group.
            await Groups.AddToGroupAsync(Context.ConnectionId,
                                         room);


            Log.Information("Connected.");
            if (!connections.ContainsKey(Context.ConnectionId))
            {
                connections.Add(Context.ConnectionId,
                                room.ToString());

                await Clients.Group(room.ToString())
                            .SendAsync(HubMethods.ReceiveMessage, $"Someone with ID '{Context.ConnectionId}' has connected");
                Log.Information("Added to connections list");
            }
                
            
           
        }
        public async Task DisconnectRoom(UserConnection userConnection)
        {
            // Disconnects
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,userConnection.RoomId.ToString());

            // test perpose ..
            foreach(var connection in connections)
            {
                Console.WriteLine(connection.ToString());
            }
            await Clients.All.SendAsync("Disconnected", $"Client with ID {userConnection.UserId} has disconnected ! ");
        }
        public async Task SendMessageRequest(UserConnection userConnection,string message)
        {

            string? roomId = userConnection.RoomId.ToString();


            if (connections.TryGetValue(Context.ConnectionId, out var connection))
            {
                await Clients.Group(roomId).SendAsync(HubMethods.SendMessage, Context.ConnectionId, $" : {message}");

            }
            
        }
    }
}
