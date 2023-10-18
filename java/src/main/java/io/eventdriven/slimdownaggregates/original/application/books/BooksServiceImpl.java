package io.eventdriven.slimdownaggregates.original.application.books;

import io.eventdriven.slimdownaggregates.original.application.books.commands.*;
import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.authors.AuthorProvider;
import io.eventdriven.slimdownaggregates.original.domain.books.publishers.PublisherProvider;
import io.eventdriven.slimdownaggregates.original.domain.books.repositories.BooksRepository;
import io.eventdriven.slimdownaggregates.original.domain.books.services.PublishingHouse;

public class BooksServiceImpl implements BooksService {
  @Override
  public void createDraft(CreateDraftCommand command) {
    var book = Book.createDraft(
      command.bookId(),
      command.title(),
      authorProvider.getOrCreate(command.author()),
      publishingHouse,
      publisherProvider.getById(command.publisherId()),
      command.edition(),
      command.genre()
    );

    repository.add(book);
  }

  @Override
  public void addChapter(AddChapterCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.addChapter(command.title(), command.content());

    repository.update(book);
  }

  @Override
  public void moveToEditing(MoveToEditingCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.moveToEditing();

    repository.update(book);
  }

  @Override
  public void addTranslation(AddTranslationCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.addTranslation(command.translation());

    repository.update(book);
  }

  @Override
  public void addFormat(AddFormatCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.addFormat(command.format());

    repository.update(book);
  }

  @Override
  public void removeFormat(RemoveFormatCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.removeFormat(command.format());

    repository.update(book);
  }

  @Override
  public void addReviewer(AddReviewerCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.addReviewer(command.reviewer());

    repository.update(book);
  }

  @Override
  public void approve(ApproveCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.approve(command.committeeApproval());

    repository.update(book);
  }

  @Override
  public void setISBN(SetISBNCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.setISBN(command.isbn());

    repository.update(book);
  }

  @Override
  public void moveToPublished(MoveToPublishedCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.moveToPublished();

    repository.update(book);
  }

  @Override
  public void moveToPrinting(MoveToPrintingCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.moveToPrinting();

    repository.update(book);
  }

  @Override
  public void moveToOutOfPrint(MoveToOutOfPrintCommand command) {
    var book = repository.findById(command.bookId())
      .orElseThrow(() -> new IllegalStateException("Book doesn't exist"));

    book.moveToOutOfPrint();

    repository.update(book);
  }


  public BooksServiceImpl(
    BooksRepository repository,
    AuthorProvider authorProvider,
    PublisherProvider publisherProvider,
    PublishingHouse publishingHouse
  ) {
    this.repository = repository;
    this.authorProvider = authorProvider;
    this.publisherProvider = publisherProvider;
    this.publishingHouse = publishingHouse;
  }

  private final BooksRepository repository;
  private final AuthorProvider authorProvider;
  private final PublisherProvider publisherProvider;
  private final PublishingHouse publishingHouse;
}
