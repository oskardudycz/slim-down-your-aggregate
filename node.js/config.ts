import dotenv from 'dotenv';
import convict from 'convict';

dotenv.config();

const convictConfig = convict({
  env: {
    format: ['prod', 'dev', 'test'],
    default: 'dev',
    arg: 'nodeEnv',
    env: 'NODE_ENV',
  },
  postgres: {
    connectionString: {
      format: String,
      default:
        "PORT = 5432; HOST = 127.0.0.1; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'",
      arg: 'POSTGRES_CONNECTION_STRING',
      env: 'POSTGRES_CONNECTION_STRING',
    },
  },
});

const env = convictConfig.get('env');
const configFileName = `./config/${env}.json`;
try {
  convictConfig.loadFile(configFileName);
} catch (exc) {
  console.warn(`Configuration file not found ('${configFileName}')`);
}

convictConfig.validate({ allowed: 'strict' }); // throws error if config does not conform to schema

export const config = convictConfig.getProperties(); // so we can operate with a plain old JavaScript object and abstract away
