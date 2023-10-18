package io.eventdriven.slimdownaggregates.original.api.config;

import io.eventdriven.slimdownaggregates.original.application.books.BooksQueryService;
import io.eventdriven.slimdownaggregates.original.application.books.BooksQueryServiceImpl;
import io.eventdriven.slimdownaggregates.original.application.books.BooksService;
import io.eventdriven.slimdownaggregates.original.application.books.BooksServiceImpl;
import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.authors.AuthorProvider;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.domain.books.factories.BookFactory;
import io.eventdriven.slimdownaggregates.original.domain.books.publishers.PublisherProvider;
import io.eventdriven.slimdownaggregates.original.domain.books.repositories.BooksQueryRepository;
import io.eventdriven.slimdownaggregates.original.domain.books.repositories.BooksRepository;
import io.eventdriven.slimdownaggregates.original.domain.books.services.PublishingHouse;
import io.eventdriven.slimdownaggregates.original.persistence.authors.AuthorEntity;
import io.eventdriven.slimdownaggregates.original.persistence.authors.AuthorRepository;
import io.eventdriven.slimdownaggregates.original.persistence.authors.AuthorService;
import io.eventdriven.slimdownaggregates.original.persistence.books.BookEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.repositories.BooksEntityRepository;
import io.eventdriven.slimdownaggregates.original.persistence.books.repositories.BooksJpaRepository;
import io.eventdriven.slimdownaggregates.original.persistence.publishers.PublisherEntity;
import io.eventdriven.slimdownaggregates.original.persistence.publishers.PublisherRepository;
import io.eventdriven.slimdownaggregates.original.persistence.publishers.PublisherService;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.support.JpaRepositoryFactoryBean;

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
  BooksRepository booksRepository(BooksJpaRepository jpaRepository, BookFactory bookFactory) {
    return new BooksEntityRepository(jpaRepository, bookFactory);
  }

  @Bean
  // assuming userId is String
  public JpaRepositoryFactoryBean<BooksJpaRepository, BookEntity, UUID> userRepository() {
    return new JpaRepositoryFactoryBean<>(BooksJpaRepository.class);
  }

  @Bean
  BookFactory bookFactory() {
    return new Book.Factory();
  }

  @Bean
  BooksQueryRepository booksQueryRepository() {
    return bookId -> Optional.empty();
  }

  @Bean
  AuthorProvider authorProvider(AuthorRepository authorRepository) {
    return new AuthorService(authorRepository);
  }

  @Bean
  // assuming userId is String
  public JpaRepositoryFactoryBean<AuthorRepository, AuthorEntity, UUID> authorRepository() {
    return new JpaRepositoryFactoryBean<>(AuthorRepository.class);
  }

  @Bean
  PublisherProvider publisherProvider(PublisherRepository publisherRepository) {
    return new PublisherService(publisherRepository);
  }

  @Bean
  // assuming userId is String
  public JpaRepositoryFactoryBean<PublisherRepository, PublisherEntity, UUID> publisherRepository() {
    return new JpaRepositoryFactoryBean<>(PublisherRepository.class);
  }

  @Bean
  PublishingHouse publishingHouse() {
    return genre -> false;
  }
}
