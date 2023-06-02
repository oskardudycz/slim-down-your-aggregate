package io.eventdriven.ecommerce.verticalslices.products.get;

import org.springframework.data.domain.PageRequest;

import java.util.List;

public record GetProducts(
  int pageNumber,
  int pageSize
) {
  public GetProducts {
    if (pageNumber < 0)
      throw new IllegalArgumentException("Page number has to be a zero-based number");

    if (pageSize < 0)
      throw new IllegalArgumentException("Page size has to be a zero-based number");
  }

  public static List<ProductShortInfo> handle(
    ProductShortInfoRepository repository,
    GetProducts query
  ) {
    return repository
      .findAll(PageRequest.of(query.pageNumber(), query.pageSize()))
      .toList();
  }
}
