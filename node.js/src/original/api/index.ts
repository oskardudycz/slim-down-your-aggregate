import initApp from './app';
import { startAPI } from './core/api';
import 'express-async-errors';

//////////////////////////////////////////////////////////
/// API
//////////////////////////////////////////////////////////

try {
  const app = initApp();
  startAPI(app);
} catch (error) {
  console.error(error);
}
