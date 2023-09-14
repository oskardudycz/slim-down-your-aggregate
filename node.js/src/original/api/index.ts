import { startAPI } from '#core/api';
import initApp from './app';

//////////////////////////////////////////////////////////
/// API
//////////////////////////////////////////////////////////

//process.once('SIGTERM', disconnectFromMongoDB);

try {
  const app = initApp();
  startAPI(app);
} catch (error) {
  console.error(error);
}
