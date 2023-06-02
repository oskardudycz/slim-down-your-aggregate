package io.eventdriven.ecommerce.verticalslices.products.get;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import org.hibernate.annotations.Immutable;

import java.util.UUID;

@Entity
@Immutable
@Table(name = "products_short_info_view")
public class ProductShortInfo {
  @Id
  private UUID id;

  @Column(nullable = false)
  private String sku;

  @Column(nullable = false)
  private String name;

  @Column
  private String description;

  public UUID getId() {
    return id;
  }

  public String getSku() {
    return sku;
  }

  public String getName() {
    return name;
  }

  public String getDescription() {
    return description;
  }
}
