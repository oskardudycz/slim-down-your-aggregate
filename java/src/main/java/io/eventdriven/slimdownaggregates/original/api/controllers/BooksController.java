package io.eventdriven.slimdownaggregates.original.api.controllers;

import io.eventdriven.slimdownaggregates.original.api.requests.AddChapterRequest;
import io.eventdriven.slimdownaggregates.original.api.requests.CreateDraftRequest;
import io.eventdriven.slimdownaggregates.original.application.books.BooksQueryService;
import io.eventdriven.slimdownaggregates.original.application.books.BooksService;
import io.eventdriven.slimdownaggregates.original.application.books.commands.AddChapterCommand;
import io.eventdriven.slimdownaggregates.original.application.books.commands.CreateDraftCommand;
import io.eventdriven.slimdownaggregates.original.application.books.commands.MoveToEditingCommand;
import io.eventdriven.slimdownaggregates.original.domain.books.authors.AuthorIdOrData;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.PositiveInt;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.UUID;

@RestController
@RequestMapping("/api/books")
public class BooksController {

  private final BooksService booksService;
  private final BooksQueryService booksQueryService;

  public BooksController(BooksService booksService, BooksQueryService booksQueryService) {
    this.booksService = booksService;
    this.booksQueryService = booksQueryService;
  }

  @PostMapping
  public ResponseEntity<UUID> createDraft(@RequestBody CreateDraftRequest request) throws URISyntaxException {
    var bookId = UUID.randomUUID();

    var author = request.author();

    booksService.createDraft(
      new CreateDraftCommand(
        new BookId(bookId),
        new Title(request.title()),
        new AuthorIdOrData(
          author.authorId() != null ? new AuthorId(author.authorId()) : null,
          author.firstName() != null ? new AuthorFirstName(author.firstName()) : null,
          author.lastName() != null ? new AuthorLastName(author.lastName()) : null
        ),
        new PublisherId(request.publisherId()),
        new PositiveInt(request.edition()),
        request.genre() != null ? new Genre(request.genre()) : null
      )
    );

    return ResponseEntity
      .created(new URI("api/books/%s".formatted(bookId)))
      .build();
  }

  @PostMapping("/{id}/chapters")
  public ResponseEntity<Void> addChapter(@PathVariable UUID id, @RequestBody AddChapterRequest request) {
    booksService.addChapter(
      new AddChapterCommand(
        new BookId(id),
        new ChapterTitle(request.title()),
        request.content() != null ? new ChapterContent(request.content()) : ChapterContent.empty
      )
    );

    return ResponseEntity.noContent().build();
  }

  @PostMapping("/{id}/move-to-editing")
  public ResponseEntity<Void> moveToEditing(@PathVariable UUID id) {
    booksService.moveToEditing(
      new MoveToEditingCommand(new BookId(id))
    );

    return ResponseEntity.noContent().build();
  }

  @GetMapping("/{id}")
  public ResponseEntity<?> findDetailsById(@PathVariable UUID id) {
    var result = booksQueryService.findDetailsById(new BookId(id));

    return result.map(ResponseEntity::ok)
      .orElse(ResponseEntity.notFound().build());
  }
}

