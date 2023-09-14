import { getApplication } from '#core/api';
import { configurePublishingHouse } from '../application';
import { BooksController } from './controllers';

const initApp = () => {
  const config = configurePublishingHouse();
  return getApplication(
    new BooksController(config.books.service, config.books.queryService),
  );
};

export default initApp;
