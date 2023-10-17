package io.eventdriven.slimdownaggregates.original.application.books.commands;


import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;

public record AddChapterCommand(
  BookId bookId,
  ChapterTitle title,
  ChapterContent content
){}
