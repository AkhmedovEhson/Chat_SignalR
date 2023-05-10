using ChatService.Domain.Models;
using ChatService.Persistence;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    public class RoomHub:Hub
    {
        private readonly ApplicationDbContext context;
        public RoomHub(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task JoinRoom(UserConnection userConnection)
        {
            var room = context.Rooms.FirstOrDefault(o => o.Id == userConnection.RoomId);
            var user = context.Users.FirstOrDefault(u => u.Id == userConnection.UserId);

            if (room == null)
                throw new Exception("Room not found");


            room.Listeners.Add(user);
            await context.SaveChangesAsync();

        }
        public async Task LeaveRoom(UserConnection userConnection)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == userConnection.UserId);
            var room = context.Rooms.FirstOrDefault(r => r.Id == userConnection.RoomId);

            if (room == null)
                return;

            user.Rooms.Remove(room);
            await context.SaveChangesAsync();

        }
    }
}
