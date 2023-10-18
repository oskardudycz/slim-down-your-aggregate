package io.eventdriven.slimdownaggregates.original.persistence.publishers;

import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface PublisherRepository extends JpaRepository<PublisherEntity, UUID> {}

