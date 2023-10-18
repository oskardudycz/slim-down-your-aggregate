package io.eventdriven.slimdownaggregates.original.persistence.core.repositories;

import io.eventdriven.slimdownaggregates.original.infrastructure.events.DomainEvent;
import jakarta.persistence.EntityNotFoundException;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public abstract class JpaEntityRepository<TAggregate, TKey, TEntity> {

  public JpaEntityRepository(JpaRepository<TEntity, TKey> jpaRepository) {
    this.jpaRepository = jpaRepository;
  }

  protected JpaRepository<TEntity, TKey> jpaRepository;

  /**
   * Find an aggregate by its ID.
   */
  public Optional<TAggregate> findById(TKey id) {
    Optional<TEntity> entity = jpaRepository.findById(id);
    return entity.map(this::mapToAggregate);
  }

  /**
   * Add a new aggregate.
   */
  public void add(TAggregate aggregate) {
    TEntity entity = mapToEntity(aggregate);
    jpaRepository.save(entity);
    // Handle domain events if needed, e.g., publishDomainEvents(aggregate.getDomainEvents());
  }

  /**
   * Update an existing aggregate.
   */
  public void update(TAggregate aggregate) {
    var entity = jpaRepository.findById(getId(aggregate));
    if (entity.isEmpty()) {
      throw new EntityNotFoundException("Entity with ID " + getId(aggregate) + " not found.");
    }
    updateEntity(entity.get(), aggregate);
    jpaRepository.save(entity.get());
    // Handle domain events if needed, e.g., publishDomainEvents(aggregate.getDomainEvents());
  }

  /**
   * Maps an entity to its corresponding aggregate.
   */
  protected abstract TAggregate mapToAggregate(TEntity entity);

  /**
   * Maps an aggregate to its corresponding entity.
   */
  protected abstract TEntity mapToEntity(TAggregate aggregate);

  /**
   * Updates the state of an entity using the state of an aggregate.
   */
  protected abstract void updateEntity(TEntity entity, TAggregate aggregate);

  /**
   * Gets the ID of an aggregate.
   */
  protected abstract TKey getId(TAggregate aggregate);

  /**
   * (Optional) Handles the publishing of domain events.
   */
  protected void publishDomainEvents(List<DomainEvent> events) {
    // Implement event publishing logic here if you have any
  }
}

