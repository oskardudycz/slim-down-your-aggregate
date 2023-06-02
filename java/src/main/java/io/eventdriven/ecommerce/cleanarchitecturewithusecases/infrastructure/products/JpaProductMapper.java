package io.eventdriven.ecommerce.cleanarchitecturewithusecases.infrastructure.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.Product;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductFactory;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.models.products.JpaProduct;

class JpaProductMapper {
  private final ProductFactory factory;

  public JpaProductMapper(ProductFactory productFactory){
    this.factory = productFactory;
  }

  public JpaProduct map(Product product) {
    return new JpaProduct(
      product.getProductId().value(),
      product.getSKU().value(),
      product.getName(),
      product.getDescription().get()
    );
  }

  public Product map(JpaProduct product) {
    return factory.register(
      new ProductId(product.getId()),
      new SKU(product.getSku()),
      product.getName(),
      product.getDescription()
    );
  }
}
