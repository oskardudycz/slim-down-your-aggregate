package io.eventdriven.slimdownaggregates.shorter.slimmed;

import io.eventdriven.slimdownaggregates.shorter.slimmed.core.FancyORM;
import io.eventdriven.slimdownaggregates.shorter.slimmed.entities.BookId;

public class ORMBookRepository implements BookRepository {
  private final FancyORM orm;

  public ORMBookRepository(FancyORM orm) {
    this.orm = orm;
  }

  @Override
  public Book find(BookId bookId) {
    return orm.find(bookId.getValue());
  }

  @Override
  public void save(BookEvent bookEvent) {
    // TODO: Add mapping to BookModel
    orm.store(bookEvent);

    orm.save();
  }
}
