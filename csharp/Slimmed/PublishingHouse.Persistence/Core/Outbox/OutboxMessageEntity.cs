using System.Text.Json;
using PublishingHouse.Core.Events;

namespace PublishingHouse.Persistence.Core.Outbox;

public class OutboxMessageEntity
{
    public long Position { get; init; } = default;
    public required string MessageId { get; init; }
    public required string MessageType { get; init; }
    public required string Data { get; init; }
    public required DateTimeOffset Scheduled { get; init; }

    public static OutboxMessageEntity From(IEventEnvelope eventEnvelope) =>
        new()
        {
            MessageId = Guid.NewGuid().ToString(),
            Data = JsonSerializer.Serialize(eventEnvelope.Event, eventEnvelope.Event.GetType()),
            Scheduled = DateTimeOffset.UtcNow,
            MessageType = eventEnvelope.Event.GetType().FullName ?? "Unknown"
        };
}
