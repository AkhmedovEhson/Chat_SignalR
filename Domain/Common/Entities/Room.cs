using ChatService.Domain.Common;
using Domain.Common;

namespace ChatService.Domain.Entities
{
    public class Room:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Listeners { get; set; }

    }
}
