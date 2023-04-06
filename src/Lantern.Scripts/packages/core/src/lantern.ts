import { ipc, Ipc } from "./ipc";

declare global {
  interface Window {
    lantern: Lantern;
  }
}

export interface Lantern {
  ipc: Ipc;
}

export const lantern: Lantern = {
  ipc,
};
