package io.eventdriven.ecommerce.cleanarchitecture.controllers.products.contracts;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import jakarta.annotation.Nullable;

public record UpdateProductRequest(
  String name,
  @Nullable String description
) {
}
