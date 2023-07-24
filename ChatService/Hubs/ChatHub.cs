using ChatService.Domain.Common;
using ChatService.Domain.Entities;
using ChatService.Domain.Models;
using ChatService.MediatR;
using ChatService.MediatR.Chat.Commands;
using ChatService.Persistence;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ChatService.Hubs
{
    public class ChatHub:Hub
    {

        private static IDictionary<string,string> connections;
        private readonly ISender _sender;



        public ChatHub(IDictionary<string,string> _connections,ISender sender) 
        {
            
            connections = _connections;
            _sender = sender;
        }









        public async Task ConnectToRoom(UserConnection connection)
        {
            var command = new ChatHubCommand(Context)
            {
                UserId = connection.UserId,
                RoomId = connection.RoomId,
            };


            await _sender.Send(command);
     
        }




        // NOTE: REFACTOR !
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
