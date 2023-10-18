package io.eventdriven.slimdownaggregates.original.persistence.authors;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.lang.NonNull;
import org.springframework.stereotype.Repository;

import java.util.Optional;
import java.util.UUID;

@Repository
public interface AuthorRepository extends JpaRepository<AuthorEntity, UUID> {
  @NonNull
  Optional<AuthorEntity> findById(@NonNull UUID id);
}

