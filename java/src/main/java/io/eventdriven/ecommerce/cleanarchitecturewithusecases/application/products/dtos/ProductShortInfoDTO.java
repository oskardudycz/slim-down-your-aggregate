package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;

public record ProductShortInfoDTO(
  ProductId productId,
  SKU sku
) {
}
