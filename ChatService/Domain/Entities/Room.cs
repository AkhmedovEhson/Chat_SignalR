namespace ChatService.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Listeners { get; set; }

    }
}
