import { DomainEvent } from '../events';

export abstract class Aggregate<TKey> {
  protected _domainEvents: DomainEvent[] = [];

  public get id() {
    return this._id;
  }

  public get domainEvents() {
    return [...this._domainEvents];
  }

  protected constructor(protected _id: TKey) {}

  protected addDomainEvent(domainEvent: DomainEvent) {
    this._domainEvents = [...this._domainEvents, domainEvent];
  }

  protected clearEvents() {
    this._domainEvents = [];
  }
}
