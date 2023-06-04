package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.BookId;
import io.eventdriven.slimdownaggregates.slimmed.entities.CommitteeApproval;

public record BookApprovedEvent(BookId id, CommitteeApproval committeeApproval) implements IDomainEvent {
}
