import request from 'supertest';
import { Application } from 'express';
import { faker } from '@faker-js/faker';
import initApp from '../app';
import { createDraft } from './booksBuilder';

describe('Publishing House', () => {
  let app: Application;

  beforeAll(() => {
    app = initApp();
  });

  describe('For existing book', () => {
    it('should add chapter', async () => {
      const existingBook = await createDraft(app);

      await request(app)
        .post(`/api/books/${existingBook.id}/chapters`)
        .send({
          title: faker.string.sample(),
          content: faker.string.sample(),
        })
        .expect(204);
    });
  });
});
