package io.eventdriven.ecommerce.verticalslices.products.get;

import org.springframework.data.repository.PagingAndSortingRepository;

import java.util.UUID;

public interface ProductShortInfoRepository
  extends PagingAndSortingRepository<ProductShortInfo, UUID> {
}
