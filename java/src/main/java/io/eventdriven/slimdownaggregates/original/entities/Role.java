package io.eventdriven.slimdownaggregates.original.entities;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;

import java.util.Collection;

@Entity
public class Role {
  @Id
  private Long id;

  public void setId(Long id) {
    this.id = id;
  }

  public Long getId() {
    return id;
  }

  public Collection<Permission> getPermissions() {
    return null;
  }

  public boolean hasPermissionTo(String permissionCode) {
    return false;
  }
}
