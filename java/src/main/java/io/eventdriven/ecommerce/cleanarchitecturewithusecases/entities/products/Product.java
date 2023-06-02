package io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;
import jakarta.annotation.Nullable;

import java.util.Optional;

public interface Product {
  ProductId getProductId();
  SKU getSKU();
  String getName();
  Optional<String> getDescription();

  void update(String name, @Nullable String description);
}
