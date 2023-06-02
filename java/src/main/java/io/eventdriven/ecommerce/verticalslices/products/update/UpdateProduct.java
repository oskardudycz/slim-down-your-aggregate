package io.eventdriven.ecommerce.verticalslices.products.update;

import io.eventdriven.ecommerce.verticalslices.products.Product;
import io.eventdriven.ecommerce.verticalslices.products.Product.*;
import jakarta.annotation.Nullable;

import java.util.Optional;
import java.util.function.Consumer;
import java.util.function.Function;

public record UpdateProduct(
  ProductId productId,
  String name,
  @Nullable String description
) {
  public static void handle(
    Function<ProductId, Optional<Product>> findProductById,
    Consumer<Product> updateProduct,
    UpdateProduct command
  ) {
    findProductById.apply(command.productId())
      .ifPresentOrElse(
        product -> {
          product.update(command.name(), command.description());

          updateProduct.accept(product);
        },
        () -> {
          throw new RuntimeException(
            "Product with id '%s%' does not exist"
              .formatted(command.productId().value())
          );
        }
      );
  }
}
