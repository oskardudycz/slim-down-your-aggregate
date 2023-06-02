package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands.UpdateProductCommand;

public interface UpdateProductUseCase {
  ProductDTO update(UpdateProductCommand createProductDTO);
}
