package io.eventdriven.ecommerce.verticalslices.products.findbyid;

import io.eventdriven.ecommerce.verticalslices.products.Product.ProductId;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.UUID;

import static io.eventdriven.ecommerce.verticalslices.products.findbyid.FindProductById.handle;

@Validated
@RestController
@RequestMapping("api/products")
public class FindProductByIdEndpoint {
  private final ProductDetailsRepository repository;

  public FindProductByIdEndpoint(
    ProductDetailsRepository repository
  ) {
    this.repository = repository;
  }

  @GetMapping("{id}")
  ResponseEntity<ProductDetails> getById(
    @PathVariable UUID id
  ) {
    return ResponseEntity.of(
      handle(repository, new FindProductById(new ProductId(id)))
    );
  }
}
