using PublishingHouse.Books.Entities;
using PublishingHouse.Core;

namespace PublishingHouse.Books.Events;

public record ChapterAddedEvent(BookId BookId, Chapter Chapter): IDomainEvent;
