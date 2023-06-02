package io.eventdriven.ecommerce.verticalslices.products.get;

import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

import static io.eventdriven.ecommerce.verticalslices.products.get.GetProducts.handle;

@Validated
@RestController
@RequestMapping("api/products")
public class GetProductsEndpoint {
  private final ProductShortInfoRepository repository;

  public GetProductsEndpoint(
    ProductShortInfoRepository repository
  ) {
    this.repository = repository;
  }

  @GetMapping
  ResponseEntity<List<ProductShortInfo>> get(
    @RequestParam int pageNumber,
    @RequestParam int pageSize
  ) {
    var result = handle(repository, new GetProducts(pageNumber, pageSize));

    return ResponseEntity.ok(result);
  }
}
