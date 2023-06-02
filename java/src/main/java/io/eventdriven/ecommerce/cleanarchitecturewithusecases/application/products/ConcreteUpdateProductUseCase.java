package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands.UpdateProductCommand;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO;

public class ConcreteUpdateProductUseCase implements UpdateProductUseCase {
  private final ProductGateway productGateway;
  private final ProductDTOMapper productDTOMapper;

  public ConcreteUpdateProductUseCase(
    ProductGateway productGateway,
    ProductDTOMapper productFactory
  ) {
    this.productGateway = productGateway;
    this.productDTOMapper = productFactory;
  }

  @Override
  public ProductDTO update(UpdateProductCommand updateProductDTO) {
    var productResult = productGateway.findById(updateProductDTO.productId());

    return productResult.map(
      product -> {
        product.update(updateProductDTO.name(), updateProductDTO.description());

        productGateway.save(product);

        var newProduct = productGateway.findById(updateProductDTO.productId()).get();

        return productDTOMapper.mapToProductDTO(newProduct);
      }
    ).orElseThrow(() ->
      new RuntimeException(
        "Product with id '%s%' does not exist"
          .formatted(updateProductDTO.productId().value())
      )
    );
  }
}
