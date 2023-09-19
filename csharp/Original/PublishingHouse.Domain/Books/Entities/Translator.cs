using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record Translator(TranslatorId Id, TranslatorName Name);
public record TranslatorId(Guid Value) : NonEmptyGuid(Value);
public record TranslatorName(string Value) : NonEmptyString(Value);
