package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.GetProductsQuery;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductShortInfoDTO;

import java.util.List;

public class ConcreteGetProductsUseCase implements GetProductsUseCase {
  private final ProductGateway productGateway;
  private final ProductDTOMapper productDTOMapper;

  public ConcreteGetProductsUseCase(
    ProductGateway productGateway,
    ProductDTOMapper productFactory
  ) {
    this.productGateway = productGateway;
    this.productDTOMapper = productFactory;
  }

  @Override
  public List<ProductShortInfoDTO> getProducts(GetProductsQuery getProductsDTO) {
    return productGateway.getAll(getProductsDTO.pageNumber(), getProductsDTO.pageSize())
      .stream()
      .map(product -> productDTOMapper.mapToProductShortInfoDTO(product))
      .toList();
  }
}
