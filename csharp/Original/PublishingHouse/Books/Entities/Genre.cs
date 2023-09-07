using PublishingHouse.Core.Validation;

namespace PublishingHouse.Books.Entities;

public class Genre
{
    public Genre(string name)
    {
        Name = name.AssertNotEmpty();
    }

    public string Name { get; }
}

