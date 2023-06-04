using SlimDownYourAggregates.Tests.Original.Core;
using SlimDownYourAggregates.Tests.Original.Entities;

namespace SlimDownYourAggregates.Tests.Original.Events;

public record BookPublishedEvent(BookId BookId, ISBN ISBN, Title Title, Author Author): IDomainEvent;
