import { DomainEvent } from '../events';

export abstract class Aggregate<TKey> {
  #id: TKey;
  #domainEvents: DomainEvent[] = [];

  public get id() {
    return this.#id;
  }

  public get domainEvents() {
    return [...this.#domainEvents];
  }

  protected constructor(id: TKey) {
    this.#id = id;
  }

  protected addDomainEvent(domainEvent: DomainEvent) {
    this.#domainEvents = [...this.#domainEvents, domainEvent];
  }

  public clearEvents() {
    this.#domainEvents = [];
  }
}
