using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
