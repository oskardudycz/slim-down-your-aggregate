package io.eventdriven.ecommerce.verticalslices.products.storage;


import org.springframework.data.repository.CrudRepository;

import java.util.UUID;

public interface JpaProductRepository
  extends CrudRepository<JpaProduct, UUID> {

  public boolean existsJpaProductBySku(String sku);
}
