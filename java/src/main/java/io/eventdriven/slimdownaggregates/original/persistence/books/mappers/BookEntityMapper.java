package io.eventdriven.slimdownaggregates.original.persistence.books.mappers;

import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.domain.books.factories.BookFactory;
import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.NonEmptyString;
import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.PositiveInt;
import io.eventdriven.slimdownaggregates.original.persistence.authors.AuthorEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.BookEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.entities.ChapterEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.entities.FormatEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.valueobjects.CommitteeApprovalVO;
import io.eventdriven.slimdownaggregates.original.persistence.books.valueobjects.TranslationVO;
import io.eventdriven.slimdownaggregates.original.persistence.publishers.PublisherEntity;
import io.eventdriven.slimdownaggregates.original.persistence.reviewers.ReviewerEntity;
import jakarta.persistence.EntityManager;

import java.util.stream.Collectors;

public class BookEntityMapper {

  public static Book mapToAggregate(BookEntity bookEntity, BookFactory bookFactory) {
    var id = new BookId(bookEntity.getId());
    var state = Book.State.valueOf(bookEntity.getCurrentState().name());
    var title = new Title(bookEntity.getTitle());
    var author = new Author(
      new AuthorId(bookEntity.getAuthor().getId()),
      new AuthorFirstName(bookEntity.getAuthor().getFirstName()),
      new AuthorLastName(bookEntity.getAuthor().getLastName())
    );
    var publisher = new Publisher(
      new PublisherId(bookEntity.getPublisher().getId()),
      new PublisherName(bookEntity.getPublisher().getName())
    );
    var edition = new PositiveInt(bookEntity.getEdition());
    var genre = bookEntity.getGenre() != null ? new Genre(bookEntity.getGenre()) : null;
    var isbn = bookEntity.getIsbn() != null ? new ISBN(bookEntity.getIsbn()) : null;
    var publicationDate = bookEntity.getPublicationDate();
    var totalPages = bookEntity.getTotalPages() != null ? new PositiveInt(bookEntity.getTotalPages()) : null;
    var numberOfIllustrations = bookEntity.getNumberOfIllustrations() != null ? new PositiveInt(bookEntity.getNumberOfIllustrations()) : null;
    var bindingType = bookEntity.getBindingType() != null ? new NonEmptyString(bookEntity.getBindingType()) : null;
    var summary = bookEntity.getSummary() != null ? new NonEmptyString(bookEntity.getSummary()) : null;
    var committeeApproval = bookEntity.getCommitteeApproval() != null
      ? new CommitteeApproval(
      bookEntity.getCommitteeApproval().isApproved(),
      new NonEmptyString(bookEntity.getCommitteeApproval().getFeedback())
    ) : null;
    var reviewers = bookEntity.getReviewers().stream()
      .map(r -> new Reviewer(new ReviewerId(r.getId()), new ReviewerName(r.getName())))
      .toList();
    var chapters = bookEntity.getChapters().stream()
      .map(c -> new Chapter(
        new ChapterNumber(c.getNumber()),
        new ChapterTitle(c.getTitle()),
        new ChapterContent(c.getContent())))
      .toList();
    var translations = bookEntity.getTranslations().stream()
      .map(c -> new Translation(
        new Language(new LanguageId(c.getLanguageId()), new LanguageName(c.getLanguage().getName())),
        new Translator(new TranslatorId(c.getTranslatorId()), new TranslatorName(c.getTranslator().getName()))
      ))
      .toList();
    var formats = bookEntity.getFormats().stream()
      .map(c -> new Format(
        new FormatType(c.getFormatType()),
        new PositiveInt(c.getTotalCopies()),
        new PositiveInt(c.getSoldCopies())
      ))
      .toList();

    return bookFactory.create(
      id,
      state,
      title,
      author,
      null, // TODO: Change that to something better
      publisher,
      edition,
      genre,
      isbn,
      publicationDate,
      totalPages,
      numberOfIllustrations,
      bindingType,
      summary,
      committeeApproval,
      reviewers,
      chapters,
      translations,
      formats
    );
  }

  public static BookEntity mapToEntity(Book book, BookEntity entity, EntityManager em) {
    entity.setId(book.id().value());
    entity.setCurrentState(BookEntity.State.valueOf(book.currentState().name()));
    entity.setTitle(book.title().value());
    entity.setGenre(book.genre() != null ? book.genre().value() : null);

    var authorEntity = em.find(AuthorEntity.class, book.author().id().value());
    entity.setAuthor(authorEntity);

    var publisherEntity = em.find(PublisherEntity.class, book.publisher().id().value());
    entity.setPublisher(publisherEntity);
    entity.setEdition(book.getEdition().value());
    entity.setIsbn(book.isbn() != null ? book.isbn().value() : null);
    entity.setPublicationDate(book.publicationDate());
    entity.setTotalPages(book.getTotalPages() != null ? book.getTotalPages().value() : null);
    entity.setNumberOfIllustrations(book.getNumberOfIllustrations() != null ? book.getNumberOfIllustrations().value() : null);
    entity.setBindingType(book.getBindingType() != null ? book.getBindingType().value() : null);
    entity.setSummary(book.getSummary() != null ? book.getSummary().value() : null);

    entity.getReviewers().clear();
    var reviewers = book.reviewers().stream()
      .map(r -> new ReviewerEntity(r.id().value(), r.name().value()))
      .toList();
    entity.setReviewers(reviewers);

    entity.getChapters().clear();
    var chapters = book.getChapters().stream()
      .map(c -> new ChapterEntity(entity.getId(), c.chapterNumber().value(), c.title().value(), c.content().value()))
      .toList();
    entity.setChapters(chapters);

    entity.getTranslations().clear();
    var translations = book.getTranslations().stream()
      .map(c -> new TranslationVO(c.language().id().value(), c.translator().id().value()))
      .toList();
    entity.setTranslations(translations);

    entity.getFormats().clear();
    var formats = book.getFormats().stream()
      .map(c -> new FormatEntity(entity.getId(), c.formatType().value(), c.totalCopies().value(), c.soldCopies().value()))
      .toList();
    entity.setFormats(formats);

    var committeeApproval = book.getCommitteeApproval() != null
      ? new CommitteeApprovalVO(
      book.getCommitteeApproval().isApproved(),
      book.getCommitteeApproval().feedback().value()
    ) : null;
    entity.setCommitteeApproval(committeeApproval);

    return entity;
  }
}
