package io.eventdriven.ecommerce.verticalslices.products.storage;

import io.eventdriven.ecommerce.verticalslices.products.Product;
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

  public JpaProduct(Product product) {
    this.id = product.getProductId().value();
    this.sku = product.getSKU().value();
    this.name = product.getName();
    this.description = product.getDescription().get();
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

  public Product toProduct(){
    return new Product(
      new Product.ProductId(id),
      new Product.SKU(sku),
      name,
      description
    );
  }
}
