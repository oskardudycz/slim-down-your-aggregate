package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;

public record FindProductByIdQuery(ProductId productId) {
}
