package io.eventdriven.slimdownaggregates.original.persistence.authors;

import io.eventdriven.slimdownaggregates.original.domain.books.authors.AuthorIdOrData;
import io.eventdriven.slimdownaggregates.original.domain.books.authors.AuthorProvider;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.Author;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.AuthorFirstName;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.AuthorId;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.AuthorLastName;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class AuthorService implements AuthorProvider {

  private final AuthorRepository authorRepository;

  public AuthorService(AuthorRepository authorRepository) {
    this.authorRepository = authorRepository;
  }

  @Transactional
  public Author getOrCreate(AuthorIdOrData authorIdOrData) {
    if (authorIdOrData.authorId() != null) {
      var entity = authorRepository.findById(authorIdOrData.authorId().value())
        .orElseThrow(() -> new IllegalArgumentException("Author not found"));

      return new Author(
        new AuthorId(entity.getId()),
        new AuthorFirstName(entity.getFirstName()),
        new AuthorLastName(entity.getLastName())
      );
    }

    var entity = new AuthorEntity();
    entity.setFirstName(authorIdOrData.firstName().value());
    entity.setLastName(authorIdOrData.lastName().value());

    var result = authorRepository.save(entity);

    return new Author(
      new AuthorId(result.getId()),
      new AuthorFirstName(entity.getFirstName()),
      new AuthorLastName(entity.getLastName())
    );
  }
}
