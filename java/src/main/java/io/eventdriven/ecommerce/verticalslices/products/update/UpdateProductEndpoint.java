package io.eventdriven.ecommerce.verticalslices.products.update;

import io.eventdriven.ecommerce.verticalslices.products.Product.*;
import io.eventdriven.ecommerce.verticalslices.products.storage.JpaProduct;
import io.eventdriven.ecommerce.verticalslices.products.storage.JpaProductRepository;
import jakarta.annotation.Nullable;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static io.eventdriven.ecommerce.verticalslices.products.update.UpdateProduct.handle;

@Validated
@RestController
@RequestMapping("api/products")
public class UpdateProductEndpoint {
  private final JpaProductRepository repository;

  public UpdateProductEndpoint(JpaProductRepository repository) {
    this.repository = repository;
  }

  @PostMapping("{id}")
  ResponseEntity<Void> update(
    @PathVariable UUID id,
    @RequestBody UpdateProductRequest request
  ) {
    handle(
      productId -> repository.findById(productId.value())
        .map(JpaProduct::toProduct),
      product -> repository.save(new JpaProduct(product)),
      new UpdateProduct(new ProductId(id), request.name(), request.description())
    );

    return ResponseEntity.ok().build();
  }

  public record UpdateProductRequest(
    String name,
    @Nullable String description
  ) {
  }
}
