export const commandHandler = async <State, Key, Command, EventType>(
  getCurrentState: (id: Key) => Promise<State | null>,
  store: (events: EventType[]) => Promise<void>,
  decider: Decider<State, Command, EventType>,
  id: Key,
  commands: Command[],
) => {
  const { decide, evolve, getInitialState } = decider;
  let aggregate: State = (await getCurrentState(id)) ?? getInitialState();
  let events: EventType[] = [];

  for (const command of commands) {
    const result = decide(command, aggregate);

    for (const event of result) {
      aggregate = evolve(aggregate, event);
    }
    events = [...events, ...result];
  }

  await store(events);
};

export type Decider<State, Command, EventType> = {
  decide: (command: Command, state: State) => EventType[];
  evolve: (state: State, event: EventType) => State;
  getInitialState: () => State;
  isTerminal?: (state: State) => boolean;
};
