namespace SlimDownYourAggregates.Tests.Original.Entities;

public class Title
{
    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        Value = value;
    }

    public string Value { get; }
}
