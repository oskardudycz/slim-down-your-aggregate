import request from 'supertest';
import { Application } from 'express';
import { faker } from '@faker-js/faker';
import { TestResponse } from '#testing/api/testResponse';
import initApp from '../app';
import { config } from '#config';

describe('Publishing House', () => {
  let app: Application;

  beforeAll(() => {
    app = initApp();
  });

  describe('For non existing book', () => {
    it('should create draft', async () => {
      const response = (await request(app)
        .post('/api/books/')
        .send({
          title: faker.string.sample(),
          author: {
            firstName: faker.person.firstName(),
            lastName: faker.person.lastName(),
          },
          publisherId: config.application.existingPublisherId,
          edition: faker.number.int({ min: 0 }),
          genre: faker.string.sample(),
        })
        .expect(201)) as TestResponse<{ id: string }>;

      const current = response.body;

      if (!current.id) {
        expect(false).toBeTruthy();
        return;
      }
      expect(current.id).toBeDefined();
    });
  });
});
