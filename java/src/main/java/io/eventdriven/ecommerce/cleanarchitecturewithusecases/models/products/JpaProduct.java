package io.eventdriven.ecommerce.cleanarchitecturewithusecases.models.products;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import java.util.UUID;

@Entity
@Table(name = "products")
public class JpaProduct {
  @Id
  private UUID id;

  @Column(nullable = false)
  private String sku;

  @Column
  private String name;

  @Column
  private String description;

  public JpaProduct(
    UUID id,
    String sku,
    String name,
    String description
  ) {
    this.id = id;
    this.sku = sku;
    this.name = name;
    this.description = description;
  }

  public JpaProduct() {

  }

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
