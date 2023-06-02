package io.eventdriven.ecommerce.cleanarchitecture.controllers.products.contracts;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.SKU;

public record ProductShortInfoResponse(
  ProductId productId,
  SKU sku
) {
}
