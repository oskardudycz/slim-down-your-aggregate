package io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.SKU;

public record ProductShortInfoDTO(
  ProductId productId,
  SKU sku
) {
}
