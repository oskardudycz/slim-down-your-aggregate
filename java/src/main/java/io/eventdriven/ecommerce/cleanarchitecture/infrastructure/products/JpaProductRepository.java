package io.eventdriven.ecommerce.cleanarchitecture.infrastructure.products;

import io.eventdriven.ecommerce.cleanarchitecture.models.products.JpaProduct;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.repository.PagingAndSortingRepository;

import java.util.UUID;

public interface JpaProductRepository
  extends PagingAndSortingRepository<JpaProduct, UUID>, CrudRepository<JpaProduct, UUID> {
}
