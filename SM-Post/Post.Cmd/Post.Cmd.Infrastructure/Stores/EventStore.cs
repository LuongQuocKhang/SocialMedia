using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Microsoft.Extensions.Configuration;
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
        private readonly IEventProducer _eventProducer;
        private readonly IConfiguration _configuration;

        public EventStore(IEventRepository eventRepository, IEventProducer eventProducer, IConfiguration configuration)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _eventProducer = eventProducer ?? throw new ArgumentNullException(nameof(eventProducer));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

                var topic = _configuration["KAFKA_TOPIC"];
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}
