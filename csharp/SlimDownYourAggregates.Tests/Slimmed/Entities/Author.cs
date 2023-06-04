namespace SlimDownYourAggregates.Tests.Slimmed.Entities;

public class Author
{
    public Author(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }
}
