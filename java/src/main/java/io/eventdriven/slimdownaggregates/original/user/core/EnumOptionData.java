package io.eventdriven.slimdownaggregates.original.user.core;

import java.io.Serializable;

public class EnumOptionData implements Serializable {
  private final Long id;
  private final String code;
  private final String value;

  public EnumOptionData(Long id, String code, String value) {
    this.id = id;
    this.code = code;
    this.value = value;
  }
}
