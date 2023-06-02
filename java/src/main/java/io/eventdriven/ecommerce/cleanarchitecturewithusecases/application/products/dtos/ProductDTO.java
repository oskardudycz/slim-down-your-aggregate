package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;
import jakarta.annotation.Nullable;

public record ProductDTO(
  ProductId productId,
  SKU sku,
  String name,
  @Nullable
  String description
) {

}
