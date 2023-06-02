package io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.SKU;
import jakarta.annotation.Nullable;

public record RegisterProductDTO(
  ProductId productId,
  SKU sku,
  String name,
  @Nullable String description
) {
}
