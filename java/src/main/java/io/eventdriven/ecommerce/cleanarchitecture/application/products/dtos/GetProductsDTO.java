package io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos;

public record GetProductsDTO(
  int pageNumber,
  int pageSize
) {
  public GetProductsDTO {
    if (pageNumber < 0)
      throw new IllegalArgumentException("Page number has to be a zero-based number");

    if (pageSize < 0)
      throw new IllegalArgumentException("Page size has to be a zero-based number");
  }
}
