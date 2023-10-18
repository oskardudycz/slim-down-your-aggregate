package io.eventdriven.slimdownaggregates.shorter.original;

import io.eventdriven.slimdownaggregates.shorter.original.entities.*;
import io.eventdriven.slimdownaggregates.shorter.original.services.PublishingHouse;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class BookApplicationService {
  private final BookRepository bookRepository;
  private final PublishingHouse publishingHouse;

  public BookApplicationService(BookRepository bookRepository, PublishingHouse publishingHouse ){
    this.bookRepository = bookRepository;
    this.publishingHouse = publishingHouse;
  }

  public void create(
    Title title,
    Author author,
    Genre genre,
    Publisher publisher,
    ISBN isbn,
    LocalDate publicationDate,
    int edition,
    int totalPages,
    int numberOfIllustrations,
    String bindingType,
    String summary
  ) {
    var book = new Book(
      new BookId(UUID.randomUUID()),
      title,
      author,
      genre,
      new ArrayList<>(),
      publishingHouse,
      publisher,
      isbn,
      publicationDate,
      edition,
      totalPages,
      numberOfIllustrations,
      bindingType,
      summary
    );

    bookRepository.save(book);
  }

  public void addChapter(BookId bookId, ChapterTitle title, ChapterContent content) {
    var book = bookRepository.find(bookId);

    book.addChapter(title, content);

    bookRepository.save(book);
  }

  public void moveToEditing(BookId bookId) {
    var book = bookRepository.find(bookId);

    book.moveToEditing();

    bookRepository.save(book);
  }

  public void addTranslation(BookId bookId, Translation translation) {
    var book = bookRepository.find(bookId);

    book.addTranslation(translation);

    bookRepository.save(book);
  }

  public void addFormat(BookId bookId, Format format) {
    var book = bookRepository.find(bookId);

    book.addFormat(format);

    bookRepository.save(book);
  }

  public void removeFormat(BookId bookId, Format format) {
    var book = bookRepository.find(bookId);

    book.removeFormat(format);

    bookRepository.save(book);
  }

  public void approve(BookId bookId, CommitteeApproval committeeApproval) {
    var book = bookRepository.find(bookId);

    book.approve(committeeApproval);

    bookRepository.save(book);
  }

  public void moveToPrinting(BookId bookId) {
    var book = bookRepository.find(bookId);

    book.moveToPrinting();

    bookRepository.save(book);
  }

  public void moveToPublished(BookId bookId) {
    var book = bookRepository.find(bookId);

    book.moveToPublished();

    bookRepository.save(book);
  }

  public void moveToOutOfPrint(BookId bookId){
    var book = bookRepository.find(bookId);

    book.moveToOutOfPrint();

    bookRepository.save(book);
  }
}
