package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands.RegisterProductCommand;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO;

class ConcreteRegisterProductUseCase implements RegisterProductUseCase {
  private final ProductGateway productGateway;
  private final ProductDTOMapper productDTOMapper;

  public ConcreteRegisterProductUseCase(
    ProductGateway productGateway,
    ProductDTOMapper productFactory
  ) {
    this.productGateway = productGateway;
    this.productDTOMapper = productFactory;
  }

  @Override
  public ProductDTO register(RegisterProductCommand createProductDTO) {
    var productExists = productGateway.productExists(createProductDTO.productId());

    if (productExists) {
      throw new RuntimeException(
        "Product with id '%s%' already exists"
          .formatted(createProductDTO.productId().value()));
    }

    productGateway.save(
      productDTOMapper.map(createProductDTO)
    );

    var newProduct = productGateway.findById(createProductDTO.productId()).get();

    return productDTOMapper.mapToProductDTO(newProduct);
  }
}
