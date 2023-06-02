package io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts;

import jakarta.annotation.Nullable;

public record UpdateProductRequest(
  String name,
  @Nullable String description
) {
}
