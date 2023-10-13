using PublishingHouse.Persistence.Languages;
using PublishingHouse.Persistence.Translators;

namespace PublishingHouse.Persistence.Books.ValueObjects;

public class TranslationVO
{
    public Guid LanguageId { get; set; }
    public LanguageEntity Language { get; set; } = default!;

    public Guid TranslatorId { get; set; }
    public TranslatorEntity Translator { get; set; } = default!;
}

