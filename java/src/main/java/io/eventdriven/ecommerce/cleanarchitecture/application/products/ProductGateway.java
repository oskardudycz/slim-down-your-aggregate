package io.eventdriven.ecommerce.cleanarchitecture.application.products;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.Product;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;

import java.util.List;
import java.util.Optional;

public interface ProductGateway {
  boolean productExists(ProductId product);
  Optional<Product> findById(ProductId product);

  List<Product> getAll(int pageNumber, int pageSize);
  void save(Product product);
}
