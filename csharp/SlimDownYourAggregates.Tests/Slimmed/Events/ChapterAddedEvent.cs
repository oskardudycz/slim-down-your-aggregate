using SlimDownYourAggregates.Tests.Slimmed.Core;
using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed.Events;

public record ChapterAddedEvent(BookId BookId, Chapter Chapter): IDomainEvent;
