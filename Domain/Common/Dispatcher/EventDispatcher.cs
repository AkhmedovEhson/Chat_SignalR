using ChatService.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Dispatcher
{
    public class EventDispatcher
    {
        private readonly IPublisher _publisher;
        public EventDispatcher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task DispatchEvent(DbContext dbContext)
        {
            var context = dbContext.ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.Events.Any())
                .Select(x => x.Entity);

            var entity = context
                .SelectMany(x=>x.Events)
                .ToList();

            context.ToList().ForEach(e => e.ClearDomainEvents());


            foreach(var item in entity)
            {
                await _publisher.Publish(item);
            }


        }

        
        

    }
}
