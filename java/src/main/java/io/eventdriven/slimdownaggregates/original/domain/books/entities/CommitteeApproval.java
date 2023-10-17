package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.NonEmptyString;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record CommitteeApproval(boolean isApproved, NonEmptyString feedback) {
  public CommitteeApproval{
    assertNotNull(feedback);
  }
}
