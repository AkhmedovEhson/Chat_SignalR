using ChatService.Domain.Entities;

namespace ChatService.Domain.Models
{
    public class UserConnection
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}
