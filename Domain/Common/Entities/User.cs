using ChatService.Domain.Common;

namespace ChatService.Domain.Entities
{
    public class User:BaseEntity
    {
        public int Id { get; set; } 
        public string? Username { get; set; }
        public string? Password { get; set; }

        public List<Room>? Rooms { get; set; }

    }
}
