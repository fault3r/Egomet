
using System;
using System.Text.Json;
using Egomet.Core.Interfaces;
using Egomet.Core.Messaging.Outbox;
using Egomet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Egomet.Infrastructure.Messaging.Outbox;

public sealed class EfPostgresEventOutbox(
    IDatabaseContext dbContext
) : IEventOutbox
{
    private readonly IDatabaseContext _context = dbContext
        ?? throw new ArgumentNullException(nameof(dbContext));

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        var messages = events.Select(domainEvent =>
        {
            ArgumentNullException.ThrowIfNull(domainEvent);

            var eventType = domainEvent.GetType();

            return new EventMessage
            {
                Id = Guid.NewGuid(),
                Type = eventType.AssemblyQualifiedName
                    ?? throw new InvalidOperationException(
                        $"Could not resolve type name for {eventType.Name}."),
                Payload = JsonSerializer
                    .Serialize(domainEvent, eventType, JsonOptions),
                Timestamp = DateTimeOffset.UtcNow,
                Published = false,
                CorrelationId = correlationId
            };
        });

        await _context.Events.AddRangeAsync(messages, cancellationToken);
    }

    public async Task<IEnumerable<EventMessage>> DequeueAsync(
        int count,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

        return await _context.Events
            .AsNoTracking()
            .Where(p => !p.Published)
            .OrderBy(p => p.Timestamp)
            .ThenBy(p => p.Id)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsPublishedAsync(
        Guid messageId,
        CancellationToken cancellationToken)
    {
        var message = await _context.Events
            .FirstOrDefaultAsync(
                p => p.Id == messageId,
                cancellationToken
            ) ?? throw new InvalidOperationException(
                $"Outbox message with ID '{messageId}' was not found.");

        message.MarkAsPublished();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
