package io.eventdriven.ecommerce.cleanarchitecture.entities.products;

import jakarta.annotation.Nullable;

import java.util.Optional;

public interface Product {
  ProductId getProductId();
  SKU getSKU();
  String getName();
  Optional<String> getDescription();

  void update(String name, @Nullable String description);
}
