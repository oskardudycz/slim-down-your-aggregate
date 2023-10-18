package io.eventdriven.slimdownaggregates.original.api.config;

import io.eventdriven.slimdownaggregates.original.application.books.BooksQueryService;
import io.eventdriven.slimdownaggregates.original.application.books.BooksQueryServiceImpl;
import io.eventdriven.slimdownaggregates.original.application.books.BooksService;
import io.eventdriven.slimdownaggregates.original.application.books.BooksServiceImpl;
import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.authors.AuthorProvider;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.domain.books.publishers.PublisherProvider;
import io.eventdriven.slimdownaggregates.original.domain.books.repositories.BooksQueryRepository;
import io.eventdriven.slimdownaggregates.original.domain.books.repositories.BooksRepository;
import io.eventdriven.slimdownaggregates.original.domain.books.services.PublishingHouse;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.UUID;

@Configuration
public class Config {

  @Bean
  BooksService booksService(
    BooksRepository repository,
    AuthorProvider authorProvider,
    PublisherProvider publisherProvider,
    PublishingHouse publishingHouse
  ) {
    return new BooksServiceImpl(
      repository,
      authorProvider,
      publisherProvider,
      publishingHouse
    );
  }

  @Bean
  BooksQueryService booksQueryService(
    BooksQueryRepository repository
  ) {
    return new BooksQueryServiceImpl(repository);
  }

  @Bean
  BooksRepository booksRepository() {
    Map<BookId, Book> books = new HashMap<>();

    return new BooksRepository() {
      @Override
      public Optional<Book> findById(BookId bookId) {
        return books.containsKey(bookId) ?
          Optional.of(books.get(bookId))
          : Optional.empty();
      }

      @Override
      public void add(Book book) {
        books.put(book.id(), book);
      }

      @Override
      public void update(Book book) {
        books.put(book.id(), book);
      }
    };
  }

  @Bean
  BooksQueryRepository booksQueryRepository() {
    return bookId -> Optional.empty();
  }

  @Bean
  AuthorProvider authorProvider() {
    return authorIdOrData ->
      authorIdOrData.authorId() != null ?
        new Author(authorIdOrData.authorId(), new AuthorFirstName("John"), new AuthorLastName("Doe"))
        : new Author(new AuthorId(UUID.randomUUID()), authorIdOrData.firstName(), authorIdOrData.lastName());
  }

  @Bean
  PublisherProvider publisherProvider() {
    return publisherId -> new Publisher(publisherId, new PublisherName("Readers Digest"));
  }

  @Bean
  PublishingHouse publishingHouse() {
    return genre -> false;
  }
}
