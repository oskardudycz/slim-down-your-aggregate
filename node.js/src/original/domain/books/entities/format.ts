import { NonEmptyString, PositiveNumber } from '#core/typing';

export type FormatType = NonEmptyString<'FormatTitle'>;

export class Format {
  readonly #type: FormatType;
  #totalCopies: PositiveNumber;
  #soldCopies: PositiveNumber;

  public get formatType() {
    return this.#type;
  }
  public get totalCopies() {
    return this.#totalCopies;
  }
  public get soldCopies() {
    return this.#soldCopies;
  }

  constructor(
    type: FormatType,
    totalCopies: PositiveNumber,
    soldCopies: PositiveNumber,
  ) {
    this.#type = type;
    this.#totalCopies = totalCopies;
    this.#soldCopies = soldCopies;
  }

  public setTotalCopies(totalCopies: PositiveNumber) {
    this.#totalCopies = totalCopies;
  }

  public setSoldCopies(soldCopies: PositiveNumber) {
    this.#soldCopies = soldCopies;
  }
}
