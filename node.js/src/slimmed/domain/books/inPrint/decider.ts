import { InPrint } from '.';
import { Published } from '../published';

export const moveToPublished = (state: InPrint): Published => {
  return {
    type: 'Published',
    data: { totalCopies: state.totalCopies },
  };
};
