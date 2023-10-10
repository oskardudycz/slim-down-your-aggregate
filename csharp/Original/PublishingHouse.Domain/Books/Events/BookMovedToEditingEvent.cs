using PublishingHouse.Books.Entities;
using PublishingHouse.Core;
using PublishingHouse.Core.Events;

namespace PublishingHouse.Books.Events;

public record BookMovedToEditingEvent(BookId BookId): IDomainEvent;
