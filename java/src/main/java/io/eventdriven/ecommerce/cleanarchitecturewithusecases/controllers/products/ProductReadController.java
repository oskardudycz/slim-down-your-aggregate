package io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products;

import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.FindProductByIdUseCase;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.GetProductsUseCase;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.FindProductByIdQuery;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.application.products.dtos.GetProductsQuery;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.ProductContractsMapper;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.ProductResponse;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.controllers.products.contracts.ProductShortInfoResponse;
import io.eventdriven.ecommerce.cleanarchitecturewithusecases.entities.products.ProductId;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@Validated
@RestController
@RequestMapping("api/products")
public class ProductReadController {
  private final ProductContractsMapper mapper;
  private final FindProductByIdUseCase findProductByIdUseCase;
  private final GetProductsUseCase getProductsUseCase;

  public ProductReadController(
    ProductContractsMapper mapper,
    FindProductByIdUseCase findProductByIdUseCase,
    GetProductsUseCase getProductsUseCase
  ) {
    this.mapper = mapper;
    this.findProductByIdUseCase = findProductByIdUseCase;
    this.getProductsUseCase = getProductsUseCase;
  }

  @GetMapping("{id}")
  ResponseEntity<ProductResponse> getById(
    @PathVariable UUID id
  ) {
    var productResult = findProductByIdUseCase.findById(
      new FindProductByIdQuery(new ProductId(id))
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
    var products = getProductsUseCase.getProducts(
      new GetProductsQuery(pageNumber, pageSize)
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
