package io.eventdriven.ecommerce.verticalslices.products.register;

import io.eventdriven.ecommerce.verticalslices.products.Product;
import io.eventdriven.ecommerce.verticalslices.products.Product.*;
import jakarta.annotation.Nullable;

import java.util.function.Consumer;
import java.util.function.Function;

public record RegisterProduct(
  ProductId productId,
  SKU sku,
  String name,
  @Nullable String description
) {
  public static void handle(
    Consumer<Product> addProduct,
    Function<SKU, Boolean> productWithSKUExists,
    RegisterProduct command
  ) {
    var product = new Product(
      command.productId(),
      command.sku(),
      command.name(),
      command.description()
    );

    if (productWithSKUExists.apply(command.sku())) {
      throw new RuntimeException(
        "Product with id '%s%' already exists"
          .formatted(command.productId().value()));
    }

    addProduct.accept(product);
  }
}
