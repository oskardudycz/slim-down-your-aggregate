using SlimDownYourAggregates.Tests.Slimmed.Core;
using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed.Events;

public record FormatRemovedEvent(
    BookId BookId,
    Format Format
): IDomainEvent;
