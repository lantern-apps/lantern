import lantern from "@lantern-app/core";

import { throwErrorIfNoLantern } from "../utils";

export function send(options: SendOptions) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.notification.send", options);
}

export interface SendOptions {
  title?: string;
  body?: string;
}
