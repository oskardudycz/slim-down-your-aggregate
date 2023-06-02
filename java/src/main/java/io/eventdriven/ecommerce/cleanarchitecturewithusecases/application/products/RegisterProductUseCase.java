package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.commands.RegisterProductCommand;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductDTO;

public interface RegisterProductUseCase {
  ProductDTO register(RegisterProductCommand createProductDTO);
}
