import { MovedToEditing } from '../bookEvent';

export function moveToEditing(
  command: MoveToEditing,
  state: Draft,
): MovedToEditing {
  const { chapters, genre } = state;

  if (chapters.length < 1) {
    throw InvalidStateError(
      'A book must have at least one chapter to move to the Editing state.',
    );
  }

  if (genre === null) {
    throw InvalidStateError(
      'Book can be moved to editing only when genre is specified',
    );
  }

  return {
    type: 'MovedToEditing',
    data: {},
  };
}
