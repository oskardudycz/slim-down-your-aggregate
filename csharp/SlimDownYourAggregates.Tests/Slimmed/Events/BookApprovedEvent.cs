using SlimDownYourAggregates.Tests.Slimmed.Core;
using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed.Events;

public record BookApprovedEvent(
    BookId BookId,
    CommitteeApproval CommitteeApproval
): IDomainEvent;
