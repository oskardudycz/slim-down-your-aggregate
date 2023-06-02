package io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos;

public record GetProductsQuery(
  int pageNumber,
  int pageSize
) {
  public GetProductsQuery {
    if (pageNumber < 0)
      throw new IllegalArgumentException("Page number has to be a zero-based number");

    if (pageSize < 0)
      throw new IllegalArgumentException("Page size has to be a zero-based number");
  }
}
