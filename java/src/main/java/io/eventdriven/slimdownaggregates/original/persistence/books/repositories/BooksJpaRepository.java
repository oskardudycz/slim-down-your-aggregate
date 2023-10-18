package io.eventdriven.slimdownaggregates.original.persistence.books.repositories;

import io.eventdriven.slimdownaggregates.original.persistence.books.BookEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface BooksJpaRepository extends JpaRepository<BookEntity, UUID> {
}
