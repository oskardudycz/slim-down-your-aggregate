namespace SlimDownYourAggregates.Tests.Original.Entities;

public class ISBN
{
    public ISBN(string number)
    {
        Number = number;
    }

    public string Number { get; }
}
