import express, { Application, NextFunction, Request, Response } from 'express';
import http from 'http';
import { ApiController } from 'src/original/infrastructure/controllers';
import 'express-async-errors';
import { problemDetailsMiddleware } from './problemDetailsMiddleware';

export const getApplication = (...controllers: ApiController[]) => {
  const app: Application = express();

  app.set('etag', false);
  app.use(express.json());
  app.use(
    express.urlencoded({
      extended: true,
    }),
  );

  app.use(...controllers.map((c) => c.router));

  app.use(problemDetailsMiddleware);

  return app;
};

export const startAPI = (app: Application, port = 5000) => {
  const server = http.createServer(app);

  server.listen(port);

  server.on('listening', () => {
    console.info('server up listening');
  });
};
