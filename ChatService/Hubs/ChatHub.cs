using Application;
using Application.Comunication;
using Application.Connect.Commands;
using ChatService.Domain.Common;
using ChatService.Domain.Entities;
using ChatService.Domain.Models;
using ChatService.MediatR;
using Domain;
using Domain.Common.Dispatcher;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ChatService.Hubs
{
    public class ChatHub:Hub
    {

        private readonly ISender _sender;



        public ChatHub(ISender sender) 
        {
            
            _sender = sender;






        }



        public async Task ConnectToRoom(UserConnection connection)
        {
            var context = new Context(this.Context,this);
            var command = new ChatHubCommand(context)
            {
                Connection = connection
            };


            await _sender.Send(command);
     
        }




        public async Task DisconnectRoom(UserConnection userConnection)
        {
            var context = new Context(this.Context, this);

            var command = new DisconnectCommand(context)
            {
                Connection = userConnection,
            };
            await _sender.Send(command);
        }



        public async Task SendMessageRequest(UserConnection userConnection,string message)
        {
            var context = new Context(Context,this);
            var command = new SendCommand(context)
            {
                Connection = userConnection,
                Message    = message
            };

            await _sender.Send(command);
            
        }
    }
}
