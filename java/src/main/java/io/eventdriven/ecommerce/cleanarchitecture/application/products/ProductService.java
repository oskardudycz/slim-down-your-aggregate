package io.eventdriven.ecommerce.cleanarchitecture.application.products;

import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.*;

import java.util.List;
import java.util.Optional;

public interface ProductService {
  ProductDTO register(RegisterProductDTO createProductDTO);
  ProductDTO update(UpdateProductDTO createProductDTO);
  Optional<ProductDTO> findById(FindProductByIdDTO findProductByIdDTO);
  List<ProductShortInfoDTO> getProducts(GetProductsDTO getProductsDTO);
}
