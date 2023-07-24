using Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatService.Domain.Common
{
    public class BaseEntity
    {
        public List<BaseEvent> Events = new() { };

        
        public void AddDomainEvent(BaseEvent domainEvent)
            => Events.Add(domainEvent);

        public void RemoveDomainEvent(BaseEvent domainEvent)
            => Events.Remove(domainEvent);

        public void ClearDomainEvents()
            => Events.Clear();
    }
}
