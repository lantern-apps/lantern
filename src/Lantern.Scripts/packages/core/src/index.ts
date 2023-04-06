import { Lantern } from "./lantern";

export type { Lantern } from "./lantern";
export type { Ipc, IpcInvoke, IpcListen, IpcUnlisten } from "./ipc";
export type {
  Observable,
  Subscription,
  Observer,
  Subscribable,
  Unsubscribable,
} from "./observable";

const lantern: Lantern = window.lantern;

export { lantern as default };
