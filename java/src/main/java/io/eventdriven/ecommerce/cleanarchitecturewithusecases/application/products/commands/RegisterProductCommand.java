package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;
import jakarta.annotation.Nullable;

public record RegisterProductCommand(
  ProductId productId,
  SKU sku,
  String name,
  @Nullable String description
) {
}
