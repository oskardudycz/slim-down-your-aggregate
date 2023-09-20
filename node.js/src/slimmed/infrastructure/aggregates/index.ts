import { DomainEvent } from '../events';

export abstract class Aggregate<TKey, TEvent extends DomainEvent> {
  #id: TKey;
  #domainEvents: TEvent[] = [];

  public get id() {
    return this.#id;
  }

  public get domainEvents() {
    return [...this.#domainEvents];
  }

  protected constructor(id: TKey) {
    this.#id = id;
  }

  protected addDomainEvent(domainEvent: TEvent) {
    this.#domainEvents = [...this.#domainEvents, domainEvent];
  }

  public clearEvents() {
    this.#domainEvents = [];
  }
}
