package io.eventdriven.ecommerce.verticalslices.products.register;

import io.eventdriven.ecommerce.verticalslices.products.Product.*;
import io.eventdriven.ecommerce.verticalslices.products.storage.JpaProduct;
import io.eventdriven.ecommerce.verticalslices.products.storage.JpaProductRepository;
import jakarta.annotation.Nullable;
import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.UUID;

import static io.eventdriven.ecommerce.verticalslices.products.register.RegisterProduct.handle;

@Validated
@RestController
@RequestMapping("api/products")
public class RegisterProductEndpoint {

  private final JpaProductRepository repository;

  public RegisterProductEndpoint(JpaProductRepository repository) {
    this.repository = repository;
  }

  @PostMapping
  @ResponseStatus(HttpStatus.CREATED)
  ResponseEntity<Void> register(
    @Valid @RequestBody RegisterProductRequest request
  ) throws URISyntaxException {
    var productId = UUID.randomUUID();

    handle(
      product -> repository.save(new JpaProduct(product)),
      sku -> repository.existsJpaProductBySku(sku.value()),
      new RegisterProduct(
        new ProductId(productId),
        new SKU(request.sku()),
        request.name(),
        request.description()
      )
    );

    return ResponseEntity
      .created(new URI("api/products/%s".formatted(productId)))
      .build();
  }

  public record RegisterProductRequest(
    String sku,
    String name,
    @Nullable String description
  ) {
  }
}
