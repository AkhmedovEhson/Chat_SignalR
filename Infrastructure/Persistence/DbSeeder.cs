using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Persistence
{
    public class DbSeeder
    {
        private readonly ModelBuilder builder;

        public DbSeeder(ModelBuilder builder)
            => this.builder = builder;

        public DbSeeder Seed() => this;

        public DbSeeder UserA()
        {
            return CreateUser(new User()
            {
                Id = 1,
                Username = "UserA",
                Password = "password",
                Rooms = new List<Room> { new Room() { Id = 1,Name = "RoomA"} }
            });

        }


        public DbSeeder CreateUser(User user)
        {
            
            builder.Entity<User>().OwnsOne(e => e.Rooms).HasData(user);
            return this;
        }
    }
}
