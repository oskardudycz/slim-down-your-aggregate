package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.Product;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;

import java.util.List;
import java.util.Optional;

public interface ProductGateway {
  boolean productExists(ProductId product);
  Optional<Product> findById(ProductId product);

  List<Product> getAll(int pageNumber, int pageSize);
  void save(Product product);
}
