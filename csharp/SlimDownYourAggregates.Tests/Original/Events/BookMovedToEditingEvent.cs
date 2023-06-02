using SlimDownYourAggregates.Tests.Original.Core;

namespace SlimDownYourAggregates.Tests.Original.Events;

public record BookMovedToEditingEvent(Guid BookId): IDomainEvent;
