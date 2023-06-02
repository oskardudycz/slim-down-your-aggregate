package io.eventdriven.ecommerce.cleanarchitecture.controllers.products.contracts;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.SKU;
import jakarta.annotation.Nullable;

import java.util.UUID;

public record ProductResponse(
  UUID productId,
  String sku,
  String name,
  @Nullable
  String description
) {

}
