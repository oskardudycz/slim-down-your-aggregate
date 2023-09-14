import express, { Application } from 'express';
import http from 'http';
import { ApiController } from 'src/original/infrastructure/controllers';

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

  return app;
};

export const startAPI = (app: Application, port = 5000) => {
  const server = http.createServer(app);

  server.listen(port);

  server.on('listening', () => {
    console.info('server up listening');
  });
};
