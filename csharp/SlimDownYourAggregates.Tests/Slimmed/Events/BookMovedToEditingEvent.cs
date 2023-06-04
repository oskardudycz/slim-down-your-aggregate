using SlimDownYourAggregates.Tests.Slimmed.Core;

namespace SlimDownYourAggregates.Tests.Slimmed.Events;

public record BookMovedToEditingEvent(Guid BookId): IDomainEvent;
