package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.PositiveInt;

public class Format {
  private final FormatType formatType;
  private PositiveInt totalCopies;
  private PositiveInt soldCopies;

  public Format(FormatType formatType, PositiveInt totalCopies, PositiveInt soldCopies)
  {
    this.formatType = formatType;
    this.totalCopies = totalCopies;
    this.soldCopies = soldCopies;
  }

  public FormatType formatType() {
    return formatType;
  }

  public PositiveInt totalCopies() {
    return totalCopies;
  }

  public PositiveInt soldCopies() {
    return soldCopies;
  }

  public void setTotalCopies(PositiveInt totalCopies) {
    this.totalCopies = totalCopies;
  }

  public void setSoldCopies(PositiveInt soldCopies) {
    this.soldCopies = soldCopies;
  }
}
