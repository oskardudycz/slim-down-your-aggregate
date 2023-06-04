using SlimDownYourAggregates.Tests.Slimmed.Core;
using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed.Events;

public record BookPublishedEvent(BookId BookId, ISBN ISBN, Title Title, Author Author): IDomainEvent;
