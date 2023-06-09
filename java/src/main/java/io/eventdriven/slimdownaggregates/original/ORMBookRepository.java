package io.eventdriven.slimdownaggregates.original;

import io.eventdriven.slimdownaggregates.original.core.FancyORM;
import io.eventdriven.slimdownaggregates.original.entities.BookId;

public class ORMBookRepository implements BookRepository {
  private final FancyORM orm  ;

  public ORMBookRepository(FancyORM orm){
    this.orm = orm;
  }

  @Override
  public Book find(BookId bookId) {
    return orm.find(bookId.getValue());
  }

  @Override
  public void save(Book book) {
    orm.store(book);

    var events = book.getDomainEvents();

    for (var event: events) {
      orm.store(event);
    }

    orm.save();
  }
}
