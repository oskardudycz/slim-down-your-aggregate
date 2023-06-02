package io.eventdriven.ecommerce.cleanarchitecturewithusecases.infrastructure.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.ProductGateway;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.Product;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import org.springframework.data.domain.PageRequest;

import java.util.List;
import java.util.Optional;

public class JpaProductGateway implements ProductGateway {
  private final JpaProductRepository repository;
  private final JpaProductMapper mapper;

  public JpaProductGateway(
    JpaProductRepository repository,
    JpaProductMapper mapper
  ){
    this.repository = repository;
    this.mapper = mapper;
  }

  @Override
  public boolean productExists(ProductId product) {
    return repository.existsById(product.value());
  }

  @Override
  public Optional<Product> findById(ProductId product) {
    return repository.findById(product.value())
      .map(jpaProduct -> mapper.map(jpaProduct));
  }

  @Override
  public List<Product> getAll(int pageNumber, int pageSize)
  {
    if (pageNumber < 0)
      throw new IllegalArgumentException("Page number has to be a zero-based number");

    if (pageSize < 0)
      throw new IllegalArgumentException("Page size has to be a zero-based number");

    return repository
      .findAll(
        PageRequest.of(pageNumber, pageSize)
      )
      .stream()
      .map(jpaProduct -> mapper.map(jpaProduct))
      .toList();
  }

  @Override
  public void save(Product product) {
    repository.save(mapper.map(product));
  }
}
