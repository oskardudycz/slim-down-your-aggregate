import { configureBooks } from './books';

export const configurePublishingHouse = () => {
  return {
    books: configureBooks(),
  };
};
