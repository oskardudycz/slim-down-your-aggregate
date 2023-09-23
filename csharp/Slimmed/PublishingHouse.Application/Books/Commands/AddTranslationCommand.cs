using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record AddTranslationCommand(BookId BookId, Translation Translation);
