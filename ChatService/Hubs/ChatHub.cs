using ChatService.Domain.Common;
using ChatService.Domain.Entities;
using ChatService.Domain.Models;
using ChatService.Persistence;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    public class ChatHub:Hub
    {

        private static IDictionary<string,string> connections;
        private readonly ApplicationDbContext context;


        public ChatHub(IDictionary<string,string> _connections,ApplicationDbContext _context) 
        {
            
            connections = _connections;
            context = _context;
        }




        //Disconnect from chat - room
        public async Task DisconnectRoom(UserConnection userConnection)
        {
            // Disconnects
            var user = context.Users.FirstOrDefault(u => u.Id == userConnection.UserId);
            await Groups.RemoveFromGroupAsync(user.Username,userConnection.RoomId.ToString());

            // test perpose ..
            foreach(var connection in connections)
            {
                Console.WriteLine(connection.ToString());
            }
        }




        //Connect to chat - room
        public async Task ConnectToRoom(UserConnection userConnection)
        {
            var user = context.Users.FirstOrDefault(o => o.Id == userConnection.UserId);
           
            var room = userConnection.RoomId.ToString();

            if (user == null)
                throw new Exception("User was not found");

            if (room is null)
                throw new Exception("Room not found");

            

            // Add to group.
            await Groups.AddToGroupAsync(Context.ConnectionId,
                                         room);


            if (!connections.ContainsKey(Context.ConnectionId))
            {
                connections.Add(Context.ConnectionId,
                                room.ToString());

                await Clients.Group(room.ToString())
                            .SendAsync(HubMethods.ReceiveMessage, "Connected");
            }
                
            
           
        }
        public async Task SendMessageRequest(UserConnection userConnection,string message)
        {

            string? roomId = userConnection.RoomId.ToString();

            var user = context.Users.First(u => u.Id == userConnection.UserId);

            if (connections.TryGetValue(Context.ConnectionId, out var connection))
            {
                await Clients.Group(roomId).SendAsync(HubMethods.SendMessage, user.Username, $" : {message}");

            }
            
        }
    }
}
