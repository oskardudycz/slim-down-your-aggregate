import { AuthorEntity } from '../authors';
import { BookEntity } from '../books/bookEntity';
import { PublisherEntity } from '../publishers/publisherEntity';

export interface DocumentsCollection<T> {
  store: (id: string, obj: T) => Promise<boolean>;
  patch: (id: string, obj: Partial<T>) => Promise<boolean>;
  delete: (id: string) => Promise<boolean>;
  get: (id: string) => Promise<T | null>;
}

export interface Database {
  collection: <T>(name: string) => DocumentsCollection<T>;
}

export const getDatabase = (): Database => {
  const storage = new Map<string, unknown>();

  return {
    collection: <T>(name: string): DocumentsCollection<T> => {
      const toFullId = (id: string) => `${name}-${id}`;

      return {
        store: (id: string, obj: T): Promise<boolean> => {
          storage.set(toFullId(id), obj);

          return Promise.resolve(true);
        },
        patch: (id: string, obj: Partial<T>): Promise<boolean> => {
          const document = storage.get(toFullId(id)) as T;

          storage.set(toFullId(id), { ...document, ...obj });

          return Promise.resolve(true);
        },
        delete: (id: string): Promise<boolean> => {
          storage.delete(toFullId(id));

          return Promise.resolve(true);
        },
        get: (id: string): Promise<T | null> => {
          const document = storage.get(toFullId(id));

          return Promise.resolve(JSON.parse(JSON.stringify(document)) as T);
        },
      };
    },
  };
};

const database = getDatabase();

export interface ORM {
  authors: DocumentsCollection<AuthorEntity>;
  books: DocumentsCollection<BookEntity>;
  publishers: DocumentsCollection<PublisherEntity>;
}

export const orm = {
  authors: database.collection<AuthorEntity>('authors'),
  books: database.collection<BookEntity>('books'),
  publishers: database.collection<PublisherEntity>('publishers'),
};
