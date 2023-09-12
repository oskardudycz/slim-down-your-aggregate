import { Aggregate } from '../../infrastructure/aggregates';
import { BookId } from './entities/bookId';

export class Book extends Aggregate<BookId> {
  #currentState: State;

  public get currentState() {
    return this.#currentState;
  }

  constructor(id: BookId, currentState: State) {
    super(id);
    this.#currentState = currentState;
  }
  // public State CurrentState { get; private set; }
  // public Title Title { get; }
  // public Author Author { get; }
  // public Genre? Genre { get; }
  // public Publisher Publisher { get; }
  // public PositiveInt Edition { get; }
  // public ISBN? ISBN { get; private set; }
  // public DateOnly? PublicationDate { get; }
  // public PositiveInt? TotalPages { get; }
  // public PositiveInt? NumberOfIllustrations { get; }
  // public NonEmptyString? BindingType { get; }
  // //TODO: add type for that
  // public NonEmptyString? Summary { get; }
  // private readonly IPublishingHouse publishingHouse;
  // public IReadOnlyList<Reviewer> Reviewers => reviewers.AsReadOnly();
  // private readonly List<Reviewer> reviewers;
  // public IReadOnlyList<Chapter> Chapters => chapters.AsReadOnly();
  // private readonly List<Chapter> chapters;
  // public CommitteeApproval? CommitteeApproval { get; private set; }
  // public IReadOnlyList<Translation> Translations => translations.AsReadOnly();
  // private readonly List<Translation> translations;
  // public IReadOnlyList<Format> Formats => formats.AsReadOnly();
  // private readonly List<Format> formats;
}

export enum State {
  Writing,
  Editing,
  Printing,
  Published,
  OutOfPrint,
}
