package io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import jakarta.annotation.Nullable;

public record UpdateProductDTO(
  ProductId productId,
  String name,
  @Nullable String description
) {
}
