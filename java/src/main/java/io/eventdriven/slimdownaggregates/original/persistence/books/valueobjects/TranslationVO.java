package io.eventdriven.slimdownaggregates.original.persistence.books.valueobjects;

import io.eventdriven.slimdownaggregates.original.persistence.languages.LanguageEntity;
import io.eventdriven.slimdownaggregates.original.persistence.translators.TranslatorEntity;
import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;
import jakarta.persistence.FetchType;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;

import java.util.UUID;

@Embeddable
public class TranslationVO {

  @Column(name = "language_id", nullable = false)
  private UUID languageId;

  @ManyToOne(fetch = FetchType.LAZY)
  @JoinColumn(name = "language_id", insertable = false, updatable = false)
  private LanguageEntity language;

  @Column(name = "translator_id", nullable = false)
  private UUID translatorId;

  @ManyToOne(fetch = FetchType.LAZY)
  @JoinColumn(name = "translator_id", insertable = false, updatable = false)
  private TranslatorEntity translator;

  // Default constructor for JPA
  public TranslationVO() {}

  // Getters and setters

  public UUID getLanguageId() {
    return languageId;
  }

  public void setLanguageId(UUID languageId) {
    this.languageId = languageId;
  }

  public LanguageEntity getLanguage() {
    return language;
  }

  public void setLanguage(LanguageEntity language) {
    this.language = language;
  }

  public UUID getTranslatorId() {
    return translatorId;
  }

  public void setTranslatorId(UUID translatorId) {
    this.translatorId = translatorId;
  }

  public TranslatorEntity getTranslator() {
    return translator;
  }

  public void setTranslator(TranslatorEntity translator) {
    this.translator = translator;
  }
}

