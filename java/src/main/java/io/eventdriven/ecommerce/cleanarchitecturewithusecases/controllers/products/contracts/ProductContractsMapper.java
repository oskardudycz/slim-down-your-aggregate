package io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands.RegisterProductCommand;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductShortInfoDTO;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands.UpdateProductCommand;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.SKU;

import java.util.UUID;

public class ProductContractsMapper {
  public RegisterProductCommand map(
    UUID productId,
    RegisterProductRequest createProductDTO
  ) {
    return new RegisterProductCommand(
      new ProductId(productId),
      new SKU(createProductDTO.sku()),
      createProductDTO.name(),
      createProductDTO.description()
    );
  }

  public UpdateProductCommand map(
    UUID productId,
    UpdateProductRequest createProductDTO
  ) {
    return new UpdateProductCommand(
      new ProductId(productId),
      createProductDTO.name(),
      createProductDTO.description()
    );
  }

  public io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.ProductResponse mapToProductResponse(
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
