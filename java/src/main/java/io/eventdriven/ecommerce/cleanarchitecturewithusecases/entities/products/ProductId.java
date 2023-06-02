package io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products;

import io.eventdriven.ecommerce.core.validation.Check;

import java.util.UUID;

public record ProductId(UUID value) {
  public ProductId {
    Check.IsNotNull(value,"ProductId");
  }
}

