package io.eventdriven.slimdownaggregates.original.user.core;

import java.io.Serializable;

import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.MappedSuperclass;
import jakarta.persistence.PostLoad;
import jakarta.persistence.PrePersist;
import jakarta.persistence.Transient;
import org.springframework.data.domain.Persistable;

/**
 * Make sure to modify the same class in the modules (fineract-investor, etc)
 * <p>
 * Abstract base class for entities.
 * <p>
 * Inspired by {@link org.springframework.data.jpa.domain.AbstractPersistable}, but Id is always Long (and this class
 * thus does not require generic parameterization), and auto-generation is of strategy
 * {@link jakarta.persistence.GenerationType#IDENTITY}.
 * <p>
 * The {@link #equals(Object)} and {@link #hashCode()} methods are NOT implemented here, which is untypical for JPA
 * (it's usually implemented based on the Id), because "we end up with issues on OpenJPA" (TODO clarify this).
 */
@MappedSuperclass
public abstract class AbstractPersistableCustom implements Persistable<Long>, Serializable {

  private static final long serialVersionUID = 9181640245194392646L;

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private Long id;

  @Override
  public Long getId() {
    return id;
  }

  @Transient
  private boolean isNew = true;

  @Override
  public boolean isNew() {
    return isNew;
  }

  @PrePersist
  @PostLoad
  void markNotNew() {
    this.isNew = false;
  }
}
