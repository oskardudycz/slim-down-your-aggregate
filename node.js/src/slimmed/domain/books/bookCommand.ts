import { Command } from 'src/slimmed/infrastructure/commands';
import {
  BookId,
  ChapterTitle,
  ChapterContent,
  Genre,
  PublisherId,
  Title,
  Author,
  Publisher,
} from './entities';
import { PositiveNumber } from '#core/typing';
import { AuthorIdOrData } from './authors';
import { Decider, commandHandler } from './commandHandler';
import { Book, Initial, State, evolve } from './book';
import { BookEvent, DraftCreated } from './bookEvent';
import { IBooksRepository } from './repositories';
import { type } from 'os';
import { AddChapter, DraftCommand, MoveToEditing, decideChapter as decideDraft } from './draft';
import { InvalidOperationError } from '#core/errors';

export type CreateDraft = Command<
  'CreateDraft',
  {
    bookId: BookId;
    title: Title;
    author: AuthorIdOrData;
    publisherId: PublisherId;
    edition: PositiveNumber;
    genre: Genre | null;
  }
>;


export type BookCommand = CreateDraft | DraftCommand;

function createDraft(
  title: Title,
  author: Author,
  publisher: Publisher,
  edition: PositiveNumber,
  genre: Genre | null,
): DraftCreated {
  return {
    type: 'DraftCreated',
    data: {
      title,
      author,
      publisher,
      edition,
      genre,
    },
  };
}



/// In Editing

addTranslation(translation: Translation): TranslationAdded {
  if (this.#translations.length >= 5) {
    throw InvalidStateError(
      'Cannot add more translations. Maximum 5 translations are allowed.',
    );
  }

  return {
    type: 'TranslationAdded',
    data: {
      translation,
    },
  };
}

addFormat(format: Format): FormatAdded {
  if (this.#formats.some((f) => f.formatType === format.formatType)) {
    throw InvalidStateError(`Format ${format.formatType} already exists.`);
  }

  return {
    type: 'FormatAdded',
    data: {
      format,
    },
  };
}

removeFormat(format: Format): FormatRemoved {
  const existingFormat = this.#formats.find(
    (f) => f.formatType === format.formatType,
  );
  if (!existingFormat) {
    throw InvalidStateError(`Format ${format.formatType} does not exist.`);
  }

  return {
    type: 'FormatRemoved',
    data: {
      format,
    },
  };
}

addReviewer(reviewer: Reviewer): ReviewerAdded {
  if (this.#reviewers.some((r) => r.name === reviewer.name)) {
    const reviewerName: string = reviewer.name;
    throw InvalidStateError(`${reviewerName} is already a reviewer.`);
  }

  return {
    type: 'ReviewerAdded',
    data: {
      reviewer,
    },
  };
}

approve(committeeApproval: CommitteeApproval): Approved {
  if (this.#reviewers.length < 3) {
    throw InvalidStateError(
      'A book cannot be approved unless it has been reviewed by at least three reviewers.',
    );
  }

  return {
    type: 'Approved',
    data: {
      committeeApproval,
    },
  };
}

setISBN(isbn: ISBN): ISBNSet {
  if (this.#isbn !== null) {
    throw InvalidStateError('Cannot change already set ISBN.');
  }

  return {
    type: 'ISBNSet',
    data: {
      isbn,
    },
  };
}

moveToPrinting(publishingHouse: IPublishingHouse): MovedToPrinting {
  if (this.#chapters.length < 1) {
    throw InvalidStateError(
      'A book must have at least one chapter to move to the printing state.',
    );
  }

  if (this.#committeeApproval === null) {
    throw InvalidStateError(
      'Cannot move to printing state until the book has been approved.',
    );
  }

  if (this.#reviewers.length < 3) {
    throw InvalidStateError(
      'A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.',
    );
  }

  if (this.#genre === null) {
    throw InvalidStateError(
      'Book can be moved to the printing only when genre is specified',
    );
  }

  // Check for genre limit
  if (!publishingHouse.isGenreLimitReached(this.#genre)) {
    throw InvalidStateError(
      'Cannot move to printing until the genre limit is reached.',
    );
  }

  return {
    type: 'MovedToPrinting',
    data: {},
  };
}

////// InPrint

moveToPublished(): Published {
  if (this.#isbn === null) {
    throw InvalidStateError('Cannot move to Published state without ISBN.');
  }

  if (this.#reviewers.length < 3) {
    throw InvalidStateError(
      'A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.',
    );
  }

  return {
    type: 'Published',
    data: {
      isbn: this.#isbn,
      title: this.#title,
      author: this.#author,
    },
  };
}

// published

moveToOutOfPrint(): MovedToOutOfPrint {
  const totalCopies = this.#formats.reduce(
    (acc, format) => acc + format.totalCopies,
    0,
  );
  const totalSoldCopies = this.#formats.reduce(
    (acc, format) => acc + format.soldCopies,
    0,
  );

  if (totalSoldCopies / totalCopies > 0.1) {
    throw InvalidStateError(
      'Cannot move to Out of Print state if more than 10% of total copies are unsold.',
    );
  }

  return {
    type: 'MovedToOutOfPrint',
    data: {},
  };
}

const decide = (command: BookCommand, state: Book): BookEvent[] => {
  const { type, data } = command;

  switch(type){
    case 'AddChapter':
    case 'MoveToEditing':
      if(state.status !== State.Writing) throw InvalidOperationError("nope!");
    
      return decideDraft(command, state);

    default:
      return [];
  }
};

const decider: Decider<Book, BookCommand, BookEvent> = {
  decide,
  evolve,
  getInitialState: () => new Initial(),
};

export const bookCommandHandler = (
  repository: IBooksRepository,
  id: BookId,
  commands: BookCommand[],
) =>
  commandHandler(
    (id) => repository.findById(id),
    (events) => repository.store(id, events),
    decider,
    id,
    commands,
  );
