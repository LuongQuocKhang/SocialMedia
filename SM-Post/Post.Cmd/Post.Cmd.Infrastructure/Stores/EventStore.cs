using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventRepository _eventRepository;

        public EventStore(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task<IEnumerable<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var eventStream = await _eventRepository.FindByAggregateId(aggregateId);

            if(eventStream == null || !eventStream.Any())
            {
                throw new AggregateNotFoundException("Incorrect Post Id provided!");
            }

            return eventStream.OrderByDescending(x => x.Id).Select(x => x.EventData).ToList();

        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventRepository.FindByAggregateId(aggregateId);

            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
            {
                // ^1 is get lastest object in list
                throw new ConcurrencyException();
            }

            var version = expectedVersion;

            foreach(var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;

                var eventModel = new EventModel()
                {
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    EventType = eventType,
                    Timestamp = DateTime.Now,
                    Version = version,
                    EventData = @event
                };

                await _eventRepository.SaveAsync(eventModel);
            }
        }
    }
}
