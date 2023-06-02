package io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.RegisterProductUseCase;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.UpdateProductUseCase;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.RegisterProductRequest;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.ProductContractsMapper;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.ProductResponse;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.UpdateProductRequest;
import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.UUID;

@Validated
@RestController
@RequestMapping("api/products")
public class ProductWriteController {
  private final ProductContractsMapper mapper;
  private final RegisterProductUseCase createProductUseCase;
  private final UpdateProductUseCase updateProductUseCase;

  public ProductWriteController(
    ProductContractsMapper mapper,
    RegisterProductUseCase createProductUseCase,
    UpdateProductUseCase updateProductUseCase
  ) {
    this.mapper = mapper;
    this.createProductUseCase = createProductUseCase;
    this.updateProductUseCase = updateProductUseCase;
  }

  @PostMapping
  @ResponseStatus(HttpStatus.CREATED)
  ResponseEntity<ProductResponse> register(
    @Valid @RequestBody RegisterProductRequest request
  ) throws URISyntaxException {
    var productId = UUID.randomUUID();

    var result = createProductUseCase.register(
      mapper.map(
        productId,
        request
      )
    );

    return ResponseEntity
      .created(new URI("api/products/%s".formatted(productId)))
      .body(mapper.mapToProductResponse(result));
  }

  @PostMapping("{id}")
  ResponseEntity<ProductResponse> update(
    @PathVariable UUID id,
    @RequestBody UpdateProductRequest request
  ) {
    var result = updateProductUseCase.update(
      mapper.map(
        id,
        request
      )
    );

    return ResponseEntity
      .ok(mapper.mapToProductResponse(result));
  }
}
