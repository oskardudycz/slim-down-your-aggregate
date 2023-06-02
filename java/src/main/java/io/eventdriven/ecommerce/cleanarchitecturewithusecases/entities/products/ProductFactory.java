package io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products;

public interface ProductFactory {
  Product register(ProductId productId, SKU sku, String name, String password);
}
