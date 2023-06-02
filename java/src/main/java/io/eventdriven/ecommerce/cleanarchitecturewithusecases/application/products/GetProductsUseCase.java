package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.GetProductsQuery;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.ProductShortInfoDTO;

import java.util.List;

public interface GetProductsUseCase {
  List<ProductShortInfoDTO> getProducts(GetProductsQuery getProductsDTO);
}
