package io.eventdriven.slimdownaggregates.original.api.requests;

import java.util.UUID;

public record CreateDraftRequest(
  String title,
  AuthorRequest author,
  UUID publisherId,
  Integer edition,
  String genre
) {
  public record AuthorRequest(UUID authorId, String firstName, String lastName) {
  }
}
