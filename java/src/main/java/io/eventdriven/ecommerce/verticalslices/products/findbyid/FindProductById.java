package io.eventdriven.ecommerce.verticalslices.products.findbyid;

import io.eventdriven.ecommerce.verticalslices.products.Product.ProductId;

import java.util.Optional;

public record FindProductById(ProductId productId) {

  public static Optional<ProductDetails> handle(
    ProductDetailsRepository repository,
    FindProductById query
  ) {
    return repository
      .findById(query.productId().value());
  }
}
