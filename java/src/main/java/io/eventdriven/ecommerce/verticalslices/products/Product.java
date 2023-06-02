package io.eventdriven.ecommerce.verticalslices.products;

import io.eventdriven.ecommerce.core.validation.Check;
import jakarta.annotation.Nullable;

import java.util.Optional;
import java.util.UUID;
import java.util.regex.Pattern;

public class Product {
  private ProductId productId;
  private SKU sku;
  private String name;
  private String description;

  public Product(
    ProductId productId,
    SKU sku,
    String name,
    @Nullable String description
  ) {
    this.productId = productId;
    this.sku = sku;
    this.name = name;
    this.description = description;
  }

  public ProductId getProductId() {
    return productId;
  }

  public SKU getSKU() {
    return sku;
  }

  public String getName() {
    return name;
  }

  public Optional<String> getDescription() {
    return Optional.ofNullable(description);
  }

  public void update(String name, String description) {
    this.name = name;
    this.description = description;
  }

  public record ProductId(UUID value) {
    public ProductId {
      Check.IsNotNull(value,"ProductId");
    }
  }

  public record SKU(String value) {
    private static final Pattern SKUPattern = Pattern.compile("[A-Z]{2,4}[0-9]{4,18}");
    public SKU {
      Check.MatchesRegexp(value, SKUPattern, "SKU");
    }
  }
}
