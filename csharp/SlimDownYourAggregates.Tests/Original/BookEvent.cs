using SlimDownYourAggregates.Tests.Original.Entities;

namespace SlimDownYourAggregates.Tests.Original;

public record WritingStarted(
    BookId BookId,
    Genre Genre,
    Title Title,
    Author Author,
    ISBN ISBN
);

public record ChapterAdded(
    BookId BookId,
    Chapter Chapter
);

public record FormatAdded(
    BookId BookId,
    Format Format
);

public record FormatRemoved(
    BookId BookId,
    Format Format
);

public record TranslationAdded(
    BookId BookId,
    Translation Translation
);

public record MovedToEditing(
    BookId BookId
);

public record Approved(
    BookId BookId,
    CommitteeApproval CommitteeApproval
);

public record MovedToPrinting
(
    BookId BookId
);

public record Published(
    BookId BookId,
    ISBN ISBN,
    Title Title,
    Author Author
);

public record MovedToOutOfPrint
(
    BookId BookId
);
