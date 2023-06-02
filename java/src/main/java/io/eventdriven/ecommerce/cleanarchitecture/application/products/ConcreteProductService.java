package io.eventdriven.ecommerce.cleanarchitecture.application.products;

import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.*;

import java.util.List;
import java.util.Optional;

public class ConcreteProductService implements ProductService {
  private final ProductGateway productGateway;
  private final ProductDTOMapper productDTOMapper;

  public ConcreteProductService(
    ProductGateway productGateway,
    ProductDTOMapper productFactory
  ) {
    this.productGateway = productGateway;
    this.productDTOMapper = productFactory;
  }

  @Override
  public ProductDTO register(RegisterProductDTO createProductDTO) {
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

  @Override
  public ProductDTO update(UpdateProductDTO updateProductDTO) {
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

  @Override
  public Optional<ProductDTO> findById(FindProductByIdDTO findProductByIdDTO) {
    return productGateway.findById(findProductByIdDTO.productId())
      .map(product -> productDTOMapper.mapToProductDTO(product));
  }

  @Override
  public List<ProductShortInfoDTO> getProducts(GetProductsDTO getProductsDTO) {
    return productGateway.getAll(getProductsDTO.pageNumber(), getProductsDTO.pageSize())
      .stream()
      .map(product -> productDTOMapper.mapToProductShortInfoDTO(product))
      .toList();
  }
}
