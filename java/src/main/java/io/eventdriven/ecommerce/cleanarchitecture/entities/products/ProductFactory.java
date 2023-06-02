package io.eventdriven.ecommerce.cleanarchitecture.entities.products;

public interface ProductFactory {
  Product register(ProductId productId, SKU sku, String name, String password);
}
