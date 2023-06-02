package io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos;

import io.eventdriven.ecommerce.cleanarchitecture.entities.products.Product;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductFactory;

public class ProductDTOMapper {
  private final ProductFactory productFactory;

  public ProductDTOMapper(ProductFactory productFactory) {
    this.productFactory = productFactory;
  }

  public Product map(RegisterProductDTO createProductDTO) {
    return productFactory.register(
      createProductDTO.productId(),
      createProductDTO.sku(),
      createProductDTO.name(),
      createProductDTO.description()
    );
  }

  public ProductDTO mapToProductDTO(Product product) {
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
