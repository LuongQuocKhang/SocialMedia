using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IConfiguration _configuration;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandler(IEventStore eventStore, IConfiguration configuration, IEventProducer eventProducer)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _eventProducer = eventProducer;
        }

        public async Task<PostAggregate> GetByIdAsync(Guid id)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsAsync(id).ConfigureAwait(false);

            if( events == null || !events.Any())
            {
                return aggregate;
            }

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(e => e.Version).Max();

            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {
            var aggregateIds = await _eventStore.GetAggregateIdsAsync();
            if (aggregateIds == null || !aggregateIds.Any())
            {
                return;
            }

            foreach (var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);
                if (aggregate == null || !aggregate.Active)
                {
                    continue;
                }

                var events = await _eventStore.GetEventsAsync(aggregateId).ConfigureAwait(false);
                if (events == null || !events.Any())
                {
                    continue;
                }

                foreach (var @event in events)
                {
                    var topic = _configuration["KAFKA_TOPIC"];
                    await _eventProducer.ProduceAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommited();
        }
    }
}
