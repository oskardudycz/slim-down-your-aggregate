import request from 'supertest';
import { Application } from 'express';
import initApp from '../app';
import { addChapter, createDraft } from './booksBuilder';

describe('Publishing House', () => {
  let app: Application;

  beforeAll(() => {
    app = initApp();
  });

  describe('For existing book with chapter', () => {
    it('should move to editing', async () => {
      const { id } = await createDraft(app);
      await addChapter(id, app);

      await request(app)
        .post(`/api/books/${id}/move-to-editing`)
        .send()
        .expect(204);
    });
  });
});
