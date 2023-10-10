using PublishingHouse.Persistence.Books.DTOs;

namespace PublishingHouse.Persistence.Books.Mappers;

public static class BookDetailsMapper
{
    public static BookDetails MapToDetails(this BookEntity entity) =>
        new BookDetails(
            entity.Id,
            entity.CurrentState.ToString(),
            entity.Title,
            new BookDetails.AuthorDetails(entity.Author.FirstName, entity.Author.LastName),
            entity.Publisher.Name,
            entity.Edition,
            entity.Genre,
            entity.ISBN,
            entity.PublicationDate,
            entity.TotalPages,
            entity.NumberOfIllustrations,
            entity.BindingType,
            entity.Summary,
            entity.CommitteeApproval != null
                ? new BookDetails.CommitteeApprovalDetails(entity.CommitteeApproval.IsApproved,
                    entity.CommitteeApproval.Feedback)
                : null,
            entity.Reviewers.Select(r => r.Name).ToList(),
            entity.Chapters.Select(c => new BookDetails.ChapterDetails(c.Title, c.Content)).ToList(),
            entity.Translations.Select(t => new BookDetails.TranslationDetails(t.Translator.Name, t.Language.Name)).ToList(),
            entity.Formats.Select(f => new BookDetails.FormatDetails(f.FormatType, f.TotalCopies, f.SoldCopies)).ToList()
        );
}
