import { NotFoundError } from '#core/errors';

export interface EntitiesCollection<T> {
  add: (id: string, obj: T) => void;
  update: (id: string, obj: T) => void;
  patch: (id: string, obj: Partial<T>) => void;
  delete: (id: string) => boolean;
  findById: (id: string) => Promise<T | null>;
}

export interface Database {
  table: <T>(name: string) => EntitiesCollection<T>;

  saveChanges(): Promise<void>;
}

export const getDatabase = (): Database => {
  let storage = new Map<string, unknown>();

  const unitOfWork = storage;

  return {
    table: <T>(name: string): EntitiesCollection<T> => {
      const toFullId = (id: string) => `${name}-${id}`;

      return {
        add: (id: string, obj: T): void => {
          if (unitOfWork.has(id))
            throw NotFoundError(
              `Entity with id ${id} already exsists in table ${name}!`,
            );

          unitOfWork.set(toFullId(id), obj);
        },
        update: (id: string, obj: T): void => {
          unitOfWork.set(toFullId(id), obj);
        },
        patch: (id: string, obj: Partial<T>): void => {
          const document = unitOfWork.get(toFullId(id)) as T;

          unitOfWork.set(toFullId(id), { ...document, ...obj });
        },
        delete: (id: string): boolean => {
          return unitOfWork.delete(toFullId(id));
        },
        findById: (id: string): Promise<T | null> => {
          const document = unitOfWork.get(toFullId(id));

          return Promise.resolve(
            document ? (JSON.parse(JSON.stringify(document)) as T) : null,
          );
        },
      };
    },
    saveChanges: (): Promise<void> => {
      storage = new Map([...storage.entries(), ...unitOfWork.entries()]);

      return Promise.resolve();
    },
  };
};
