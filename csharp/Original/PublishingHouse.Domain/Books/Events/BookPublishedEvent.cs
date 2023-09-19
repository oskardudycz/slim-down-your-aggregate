using PublishingHouse.Books.Entities;
using PublishingHouse.Core;

namespace PublishingHouse.Books.Events;

public record BookPublishedEvent(BookId BookId, ISBN ISBN, Title Title, Author Author): IDomainEvent;
