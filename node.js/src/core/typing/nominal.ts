import { Brand } from './brand';
import { Flavour } from './flavour';

export type Nominal<T, NameT> = T extends object
  ? Flavour<T, NameT>
  : Brand<T, NameT>;
