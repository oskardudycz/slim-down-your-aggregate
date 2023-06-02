package io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ConcreteProduct;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.Product;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductFactory;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;

class ConcreteProductFactory implements ProductFactory {
  @Override
  public Product register(ProductId productId, SKU sku, String name, String password) {
    return new io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ConcreteProduct(productId, sku, name, password);
  }
}
