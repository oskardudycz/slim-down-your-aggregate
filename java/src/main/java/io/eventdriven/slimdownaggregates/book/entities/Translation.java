package io.eventdriven.slimdownaggregates.book.entities;

public class Translation {
  private final Language language;
  private final Translator translator;

  public Translation(Language language, Translator translator)
  {
    this.language = language;
    this.translator = translator;
  }
}
