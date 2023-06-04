using SlimDownYourAggregates.Tests.Original.Core;
using SlimDownYourAggregates.Tests.Original.Entities;

namespace SlimDownYourAggregates.Tests.Original.Events;

public record BookMovedToEditingEvent(BookId BookId): IDomainEvent;
