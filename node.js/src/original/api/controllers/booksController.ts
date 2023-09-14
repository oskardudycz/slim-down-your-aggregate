import { NextFunction, Request, Response, Router } from 'express';
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
import { BookId } from 'src/original/domain/books/entities';
import { sendCreated } from '#core/http';

export class BooksController extends ApiController {
  constructor(
    private booksService: IBooksService,
    private bookQueryService: IBooksQueryService,
  ) {
    super();
  }

  protected routes(router: Router): void {
    router.post('/api/books/', this.createDraft);
    router.post('/api/books/:bookId/chapters', this.addChapter);
    router.post('/api/books/:bookId/move-to-editing', this.moveToEditing);
    router.get('/api/books/:bookId', this.findDetailsById);
  }

  public createDraft = async (
    request: CreateDraftRequest,
    response: Response,
    next: NextFunction,
  ) => {
    try {
      const bookId: BookId = nonEmptyString(uuid());

      const command = toCreateDraftCommand(bookId, request);

      await this.booksService.createDraft(command);

      sendCreated(response, bookId);
    } catch (error) {
      console.error(error);
      next(error);
    }
  };

  public addChapter = async (
    request: AddChapterRequest,
    response: Response,
    next: NextFunction,
  ) => {
    try {
      const command = toAddChapterCommand(request);

      await this.booksService.addChapter(command);

      response.sendStatus(204);
    } catch (error) {
      console.error(error);
      next(error);
    }
  };

  public moveToEditing = async (
    request: AddChapterRequest,
    response: Response,
    next: NextFunction,
  ) => {
    try {
      const command = toMoveToEditingCommand(request);

      await this.booksService.moveToEditing(command);

      response.sendStatus(204);
    } catch (error) {
      console.error(error);
      next(error);
    }
  };

  private findDetailsById = async (
    request: Request,
    response: Response,
    next: NextFunction,
  ) => {
    try {
      const query = toFindDetailsByIdQuery(request);

      const result = await this.bookQueryService.findDetailsById(query);

      response.send(result);
    } catch (error) {
      console.error(error);
      next(error);
    }
  };
}
