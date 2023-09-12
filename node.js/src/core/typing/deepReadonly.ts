export type DeepReadonly<TInput> = {
  readonly [Key in keyof TInput]: TInput[Key] extends object
    ? DeepReadonly<TInput[Key]>
    : TInput[Key];
};
