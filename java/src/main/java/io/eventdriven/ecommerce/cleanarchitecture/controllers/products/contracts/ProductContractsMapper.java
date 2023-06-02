package io.eventdriven.ecommerce.cleanarchitecture.controllers.products.contracts;

import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.RegisterProductDTO;
import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.ProductDTO;
import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.ProductShortInfoDTO;
import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.UpdateProductDTO;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductFactory;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.SKU;

import java.util.UUID;

public class ProductContractsMapper {
  public RegisterProductDTO map(
    UUID productId,
    RegisterProductRequest createProductDTO
  ) {
    return new RegisterProductDTO(
      new ProductId(productId),
      new SKU(createProductDTO.sku()),
      createProductDTO.name(),
      createProductDTO.description()
    );
  }

  public UpdateProductDTO map(
    UUID productId,
    UpdateProductRequest createProductDTO
  ) {
    return new UpdateProductDTO(
      new ProductId(productId),
      createProductDTO.name(),
      createProductDTO.description()
    );
  }

  public ProductResponse mapToProductResponse(
    ProductDTO product
  ) {
    return new ProductResponse(
      product.productId().value(),
      product.sku().value(),
      product.name(),
      product.description()
    );
  }


  public ProductShortInfoResponse mapToProductShortInfoResponse(
    ProductShortInfoDTO product
  ) {
    return new ProductShortInfoResponse(
      product.productId(),
      product.sku()
    );
  }
}
