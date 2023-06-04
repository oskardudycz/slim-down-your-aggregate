using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed.Services;

public interface IPublishingHouse
{
    bool IsGenreLimitReached(Genre genre);
}

