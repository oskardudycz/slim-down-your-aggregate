package io.eventdriven.slimdownaggregates.original.domain.books.dtos;

import java.time.LocalDate;
import java.util.UUID;

public record BookDetails(
  UUID id,
  String currentState,
  String title,
  AuthorDetails author,
  String publisherName,
  int edition,
  String genre,
  String isbn,
  LocalDate publicationDate,
  Integer totalPages,
  Integer numberOfIllustrations,
  String bindingType,
  String summary,
  CommitteeApprovalDetails committeeApproval,
  String[] reviewers,
  ChapterDetails[] chapters,
  TranslationDetails[] translations,
  FormatDetails[] formats

) {

  public record AuthorDetails(String firstName, String lastName) {
  }

  public record CommitteeApprovalDetails(boolean isApproved, String feedback) {
  }

  public record ChapterDetails(String title, String content) {
  }

  public record TranslationDetails(String language, String translator) {
  }

  public record FormatDetails(String formatType, int totalCopies,
                              int soldCopies) {
  }
}
