using PublishingHouse.Persistence.Languages;
using PublishingHouse.Persistence.Translators;

namespace PublishingHouse.Persistence.Books.ValueObjects;

public class Translation
{
    public Translation(LanguageEntity language, TranslatorEntity translatorEntity)
    {
        Language = language;
        TranslatorEntity = translatorEntity;
    }

    public LanguageEntity Language { get; }
    public TranslatorEntity TranslatorEntity { get; }
}

