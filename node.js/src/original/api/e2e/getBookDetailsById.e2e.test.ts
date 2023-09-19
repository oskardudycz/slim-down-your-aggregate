import request from 'supertest';
import { Application } from 'express';
import { faker } from '@faker-js/faker';
import initApp from '../app';
import { EXISTING_PUBLISHER_ID } from 'src/original/application/books';
import { createDraft } from './booksBuilder';

describe('Publishing House', () => {
  let app: Application;

  beforeAll(() => {
    app = initApp();
  });

  describe('For non existing book', () => {
    const unknownBookId = faker.string.uuid();

    it('should return not found', () => {
      return request(app)
        .get(`/api/books/${unknownBookId}`)
        .send({
          title: faker.string.sample(),
          author: {
            firstName: faker.person.firstName(),
            lastName: faker.person.lastName(),
          },
          publisherId: EXISTING_PUBLISHER_ID,
          edition: faker.number.int({ min: 0 }),
          genre: faker.string.sample(),
        })
        .expect(404);
    });
  });

  describe('For existing book', () => {
    it('should return OK', async () => {
      const existingBook = await createDraft(app);
      const { publisherId: _publisherId, ...expectedBody } = existingBook;

      const response = await request(app)
        .get(`/api/books/${expectedBody.id}`)
        .expect(200);

      expect(response.body).toMatchObject(expectedBody);
    });
  });
});
