using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Core.Events;

public interface IEventEnvelope
{
    object Event { get; }
    EventMetadata Metadata { get; }
}

public record EventEnvelope<TEvent>(TEvent Event, EventMetadata Metadata): IEventEnvelope
    where TEvent: notnull
{
    object IEventEnvelope.Event => Event;
}

public record EventMetadata(NonEmptyGuid RecordId);
