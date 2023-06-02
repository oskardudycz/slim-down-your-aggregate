package io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;
import jakarta.annotation.Nullable;

import java.util.UUID;

public record RegisterProductRequest(
  String sku,
  String name,
  @Nullable String description
) {
}
