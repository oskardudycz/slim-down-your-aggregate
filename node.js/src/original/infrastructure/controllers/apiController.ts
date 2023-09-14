import { Router } from 'express';

export abstract class ApiController {
  #router: Router = Router();

  get router() {
    return this.#router;
  }

  constructor() {
    this.routes(this.#router);
  }

  protected abstract routes(router: Router): void;
}
