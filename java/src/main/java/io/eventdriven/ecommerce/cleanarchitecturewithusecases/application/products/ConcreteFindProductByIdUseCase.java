package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.*;

import java.util.Optional;

class ConcreteFindProductByIdUseCase implements FindProductByIdUseCase {
  private final ProductGateway productGateway;
  private final ProductDTOMapper productDTOMapper;

  public ConcreteFindProductByIdUseCase(
    ProductGateway productGateway,
    ProductDTOMapper productFactory
  ) {
    this.productGateway = productGateway;
    this.productDTOMapper = productFactory;
  }

  @Override
  public Optional<ProductDTO> findById(FindProductByIdQuery findProductByIdDTO) {
    return productGateway.findById(findProductByIdDTO.productId())
      .map(product -> productDTOMapper.mapToProductDTO(product));
  }
}
