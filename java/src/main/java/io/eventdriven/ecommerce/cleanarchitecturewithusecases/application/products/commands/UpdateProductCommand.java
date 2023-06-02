package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import jakarta.annotation.Nullable;

public record UpdateProductCommand(
  ProductId productId,
  String name,
  @Nullable String description
) {
}
