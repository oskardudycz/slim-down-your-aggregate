package io.eventdriven.ecommerce.cleanarchitecture.controllers.products;

import io.eventdriven.ecommerce.cleanarchitecture.application.products.ProductService;
import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.FindProductByIdDTO;
import io.eventdriven.ecommerce.cleanarchitecture.application.products.dtos.GetProductsDTO;
import io.eventdriven.ecommerce.cleanarchitecture.controllers.products.contracts.*;
import io.eventdriven.ecommerce.cleanarchitecture.entities.products.ProductId;
import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.List;
import java.util.UUID;

@Validated
@RestController
@RequestMapping("api/products")
public class ProductController {
  private final ProductContractsMapper mapper;
  private final ProductService productService;

  public ProductController(
    ProductContractsMapper mapper,
    ProductService productService
  ) {
    this.mapper = mapper;
    this.productService = productService;
  }

  @PostMapping
  @ResponseStatus(HttpStatus.CREATED)
  ResponseEntity<ProductResponse> register(
    @Valid @RequestBody RegisterProductRequest request
  ) throws URISyntaxException {
    var productId = UUID.randomUUID();

    var result = productService.register(
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
    var result = productService.update(
      mapper.map(
        id,
        request
      )
    );

    return ResponseEntity
      .ok(mapper.mapToProductResponse(result));
  }


  @GetMapping("{id}")
  ResponseEntity<ProductResponse> getById(
    @PathVariable UUID id
  ) {
    var productResult = productService.findById(
      new FindProductByIdDTO(new ProductId(id))
    );

    return ResponseEntity.of(
      productResult.map(
        product -> mapper.mapToProductResponse(product)
      )
    );
  }

  @GetMapping
  ResponseEntity<List<ProductShortInfoResponse>> get(
    @RequestParam int pageNumber,
    @RequestParam int pageSize
  ) {
    var products = productService.getProducts(
      new GetProductsDTO(pageNumber, pageSize)
    );

    return ResponseEntity.ok(
      products.stream()
        .map(
          product -> mapper.mapToProductShortInfoResponse(product)
        )
        .toList()
    );
  }
}
