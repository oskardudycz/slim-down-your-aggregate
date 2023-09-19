import { configurePublishingHouse } from '../application';
import { BooksController } from './controllers';
import { getApplication } from './core/api';

const initApp = () => {
  const config = configurePublishingHouse();
  return getApplication(
    new BooksController(config.books.service, config.books.queryService),
  );
};

export default initApp;
