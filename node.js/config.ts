import dotenv from 'dotenv';
import convict from 'convict';
import { v4 as uuid } from 'uuid';

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
  application: {
    existingPublisherId: {
      format: String,
      default: uuid(),
      arg: 'EXISTING_PUBLISHER_ID',
      env: 'EXISTING_PUBLISHER_ID',
    },
    existingPublisherName: {
      format: String,
      default: uuid(),
      arg: 'EXISTING_PUBLISHER_NAME',
      env: 'EXISTING_PUBLISHER_NAME',
    },
    minimumReviewersRequiredForApproval: {
      format: Number,
      default: 3,
      arg: 'MINIMUM_REVIEWERS_REQUIRED_FOR_APPROVAL',
      env: 'MINIMUM_REVIEWERS_REQUIRED_FOR_APPROVAL',
    },
    maximumNumberOfTranslations: {
      format: Number,
      default: 5,
      arg: 'MAXIMUM_NUMBER_OF_TRANSLATIONS',
      env: 'MAXIMUM_NUMBER_OF_TRANSLATIONS',
    },
    maxAllowedUnsoldCopiesRatioToGoOutOfPrint: {
      format: Number,
      default: 0.1,
      arg: 'MAX_ALLOWED_UNSOLD_COPIES_RATIO_TO_GO_OUT_OF_PRINT',
      env: 'MAX_ALLOWED_UNSOLD_COPIES_RATIO_TO_GO_OUT_OF_PRINT',
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
