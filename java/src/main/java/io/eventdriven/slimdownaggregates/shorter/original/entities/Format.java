package io.eventdriven.slimdownaggregates.shorter.original.entities;

public class Format {
  private final String formatType;
  private final int totalCopies;
  private final int soldCopies;

  public Format(String formatType, int totalCopies, int soldCopies)
  {
    this.formatType = formatType;
    this.totalCopies = totalCopies;
    this.soldCopies = soldCopies;
  }

  public String getFormatType() {
    return formatType;
  }

  public int getTotalCopies() {
    return totalCopies;
  }

  public int getSoldCopies() {
    return soldCopies;
  }
}
