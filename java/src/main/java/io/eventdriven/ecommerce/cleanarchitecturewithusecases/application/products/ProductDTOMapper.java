package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands.RegisterProductCommand;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductShortInfoDTO;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.Product;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductFactory;

public class ProductDTOMapper {
  private final ProductFactory productFactory;

  public ProductDTOMapper(ProductFactory productFactory) {
    this.productFactory = productFactory;
  }

  public Product map(RegisterProductCommand createProductDTO) {
    return productFactory.register(
      createProductDTO.productId(),
      createProductDTO.sku(),
      createProductDTO.name(),
      createProductDTO.description()
    );
  }

  public io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO mapToProductDTO(Product product) {
    return new ProductDTO(
      product.getProductId(),
      product.getSKU(),
      product.getName(),
      product.getDescription().get()
    );
  }


  public ProductShortInfoDTO mapToProductShortInfoDTO(Product product) {
    return new ProductShortInfoDTO(
      product.getProductId(),
      product.getSKU()
    );
  }
}
