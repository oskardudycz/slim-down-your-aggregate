package io.eventdriven.slimdownaggregates.original.application.books;

import io.eventdriven.slimdownaggregates.original.application.books.commands.*;

public interface BooksService {
  void createDraft(CreateDraftCommand command);
  void addChapter(AddChapterCommand command);
  void moveToEditing(MoveToEditingCommand command);

  void addTranslation(AddTranslationCommand command);
  void addFormat(AddFormatCommand command);
  void removeFormat(RemoveFormatCommand command);
  void addReviewer(AddReviewerCommand command);
  void approve(ApproveCommand command);
  void setISBN(SetISBNCommand command);
  void moveToPublished(MoveToPublishedCommand command);
  void moveToPrinting(MoveToPrintingCommand command);
  void moveToOutOfPrint(MoveToOutOfPrintCommand command);
}
