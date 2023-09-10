using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record AddChapterCommand(BookId BookId, ChapterTitle Title, ChapterContent Content);
