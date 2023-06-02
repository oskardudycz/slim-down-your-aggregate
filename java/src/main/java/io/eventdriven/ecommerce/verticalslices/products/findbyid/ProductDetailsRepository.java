package io.eventdriven.ecommerce.verticalslices.products.findbyid;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.models.products.JpaProduct;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.repository.PagingAndSortingRepository;
import org.springframework.data.repository.Repository;

import java.util.Optional;
import java.util.UUID;

public interface ProductDetailsRepository extends Repository<JpaProduct, UUID> {
  Optional<ProductDetails> findById(UUID id);
}
