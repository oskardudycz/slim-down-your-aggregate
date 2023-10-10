using PublishingHouse.Books.Entities;
using PublishingHouse.Core;
using PublishingHouse.Core.Events;

namespace PublishingHouse.Books.Events;

public record ChapterAddedEvent(BookId BookId, Chapter Chapter): IDomainEvent;
