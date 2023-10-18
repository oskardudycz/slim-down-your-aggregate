package io.eventdriven.slimdownaggregates.original.persistence.books.repositories;

import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;
import io.eventdriven.slimdownaggregates.original.domain.books.factories.BookFactory;
import io.eventdriven.slimdownaggregates.original.domain.books.repositories.BooksRepository;
import io.eventdriven.slimdownaggregates.original.persistence.books.BookEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.mappers.BookEntityMapper;
import io.eventdriven.slimdownaggregates.original.persistence.core.repositories.JpaEntityRepository;
import jakarta.persistence.PersistenceContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;
import org.springframework.data.jpa.repository.EntityGraph;
import jakarta.persistence.EntityManager;

import java.util.Optional;
import java.util.UUID;

@Repository
public class BooksEntityRepository extends JpaEntityRepository<Book, UUID, BookEntity> implements BooksRepository {

  @PersistenceContext
  private EntityManager entityManager;

  private final BookFactory bookFactory;

  @Autowired
  public BooksEntityRepository(BooksJpaRepository jpaRepository, BookFactory bookFactory) {
      super(jpaRepository);
      this.bookFactory = bookFactory;
  }

  @Override
  @EntityGraph(attributePaths = {
    "author",
    "publisher",
    "reviewers",
    "chapters",
    "translations",
    "formats"
  })
  public Optional<Book> findById(BookId id) {
    return super.findById(id.value());
  }

  @Override
  protected Book mapToAggregate(BookEntity entity) {
    return BookEntityMapper.mapToAggregate(entity, bookFactory);
  }

  @Override
  protected BookEntity mapToEntity(Book aggregate) {
    return BookEntityMapper.mapToEntity(aggregate, new BookEntity(), entityManager);
  }

  @Override
  protected void updateEntity(BookEntity entity, Book aggregate) {
      BookEntityMapper.mapToEntity(aggregate, entity, entityManager);
  }

  @Override
  protected UUID getId(Book aggregate) {
    return aggregate.id().value();
  }
}

