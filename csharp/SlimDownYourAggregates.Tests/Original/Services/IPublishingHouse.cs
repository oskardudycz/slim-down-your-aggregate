using SlimDownYourAggregates.Tests.Original.Entities;

namespace SlimDownYourAggregates.Tests.Original.Services;

public interface IPublishingHouse
{
    bool IsGenreLimitReached(Genre genre);
}

