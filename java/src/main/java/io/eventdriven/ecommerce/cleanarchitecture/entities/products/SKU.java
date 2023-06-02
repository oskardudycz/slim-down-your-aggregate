package io.eventdriven.ecommerce.cleanarchitecture.entities.products;

import io.eventdriven.ecommerce.core.validation.Check;

import java.util.regex.Pattern;

public record SKU(String value) {
  private static final Pattern SKUPattern = Pattern.compile("[A-Z]{2,4}[0-9]{4,18}");
  public SKU {
    Check.MatchesRegexp(value, SKUPattern, "SKU");
  }
}
