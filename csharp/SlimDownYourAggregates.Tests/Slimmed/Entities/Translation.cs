namespace SlimDownYourAggregates.Tests.Slimmed.Entities;

public class Translation
{
    public Translation(Language language, Translator translator)
    {
        Language = language;
        Translator = translator;
    }

    public Language Language { get; }
    public Translator Translator { get; }
}

