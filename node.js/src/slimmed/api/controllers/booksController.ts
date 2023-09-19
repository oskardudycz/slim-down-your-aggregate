import { IRouter, Request, Response } from 'express';
import { v4 as uuid } from 'uuid';
import { ApiController } from '../../infrastructure/controllers';
import { IBooksService } from '../../application/books/booksService';
import { IBooksQueryService } from '../../application/books/booksQueryService';
import {
  AddChapterRequest,
  CreateDraftRequest,
  toAddChapterCommand,
  toCreateDraftCommand,
  toMoveToEditingCommand,
  toFindDetailsByIdQuery,
} from '../requests';
import { nonEmptyString } from '#core/typing';
import { sendCreated } from '../core/http';
import { BookId } from '../../domain/books/entities';

export class BooksController extends ApiController {
  constructor(
    private booksService: IBooksService,
    private bookQueryService: IBooksQueryService,
  ) {
    super();
  }

  protected routes(router: IRouter): void {
    router.post('/api/books/', (req, res) => this.createDraft(req, res));
    router.post(
      '/api/books/:bookId/chapters',
      async (req, res) => await this.addChapter(req, res),
    );
    router.post('/api/books/:bookId/move-to-editing', (req, res) =>
      this.moveToEditing(req, res),
    );
    router.get(
      '/api/books/:bookId',
      async (req, res) => await this.findDetailsById(req, res),
    );
  }

  public createDraft = async (
    request: CreateDraftRequest,
    response: Response,
  ) => {
    const bookId: BookId = nonEmptyString(uuid());

    const command = toCreateDraftCommand(bookId, request);

    await this.booksService.createDraft(command);

    sendCreated(response, bookId);
  };

  public addChapter = async (
    request: AddChapterRequest,
    response: Response,
  ) => {
    const command = toAddChapterCommand(request);

    await this.booksService.addChapter(command);

    response.sendStatus(204);
  };

  public moveToEditing = async (
    request: AddChapterRequest,
    response: Response,
  ) => {
    const command = toMoveToEditingCommand(request);

    await this.booksService.moveToEditing(command);

    response.sendStatus(204);
  };

  private findDetailsById = async (request: Request, response: Response) => {
    const query = toFindDetailsByIdQuery(request);

    const result = await this.bookQueryService.findDetailsById(query);

    if (!result) response.sendStatus(404);

    response.send(result);
  };
}
