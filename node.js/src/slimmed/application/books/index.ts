import {
  BookQueryRepository,
  BooksRepository,
} from '../../persistence/books/repositories';
import { BooksService } from './booksService';
import { AuthorProvider } from '../../persistence/authors';
import { PublisherProvider } from '../../persistence/publishers';
import { BooksQueryService } from './booksQueryService';
import { BookFactory } from '../../domain/books/book';
import {
  PublishingHouseOrm,
  publishingHouseOrm,
} from '../../persistence/publishingHouseOrm';
import { positiveNumber } from '#core/typing';
import { config } from '#config';
import { ratio } from '#core/typing/ratio';

const {
  existingPublisherId,
  existingPublisherName,
  maximumNumberOfTranslations,
  minimumReviewersRequiredForApproval,
  maxAllowedUnsoldCopiesRatioToGoOutOfPrint,
} = config.application;

const seedPublishingHouse = (orm: PublishingHouseOrm) => {
  orm.publishers.add(existingPublisherId, {
    id: existingPublisherId,
    name: existingPublisherName,
  });
};

export const configureBooks = () => {
  const orm = publishingHouseOrm(seedPublishingHouse);
  return {
    service: new BooksService(
      new BooksRepository(orm),
      new BookFactory(),
      new AuthorProvider(orm),
      new PublisherProvider(orm),
      {
        isGenreLimitReached: () => true,
      },
      positiveNumber(minimumReviewersRequiredForApproval),
      positiveNumber(maximumNumberOfTranslations),
      ratio(maxAllowedUnsoldCopiesRatioToGoOutOfPrint),
    ),
    queryService: new BooksQueryService(new BookQueryRepository(orm)),
  };
};
