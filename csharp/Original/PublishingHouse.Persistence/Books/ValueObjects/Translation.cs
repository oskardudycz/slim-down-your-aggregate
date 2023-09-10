using PublishingHouse.Persistence.Languages;
using PublishingHouse.Persistence.Translators;

namespace PublishingHouse.Persistence.Books.ValueObjects;

public class Translation
{
    public required LanguageEntity Language { get; set; }
    public required TranslatorEntity Translator { get; set; }
}

