package io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;

public record ProductShortInfoResponse(
  ProductId productId,
  SKU sku
) {
}
