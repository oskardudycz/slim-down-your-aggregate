using PublishingHouse.Books.Entities;
using PublishingHouse.Core;

namespace PublishingHouse.Books.Events;

public record BookMovedToEditingEvent(BookId BookId): IDomainEvent;
