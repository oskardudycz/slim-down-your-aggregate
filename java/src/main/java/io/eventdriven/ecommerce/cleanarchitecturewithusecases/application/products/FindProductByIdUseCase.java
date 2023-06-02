package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.FindProductByIdQuery;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO;

import java.util.Optional;

public interface FindProductByIdUseCase {
  Optional<ProductDTO> findById(FindProductByIdQuery findProductByIdDTO);
}
