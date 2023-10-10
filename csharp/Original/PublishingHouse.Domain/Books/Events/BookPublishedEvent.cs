using PublishingHouse.Books.Entities;
using PublishingHouse.Core.Events;

namespace PublishingHouse.Books.Events;

public record BookPublishedEvent(BookId BookId, ISBN ISBN, Title Title, Author Author): IDomainEvent;
