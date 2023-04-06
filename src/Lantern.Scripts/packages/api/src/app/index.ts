import lantern from "@lantern-app/core";

import { throwErrorIfNoLantern } from "../utils";

export function shutdown() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.app.shutdown");
}

export function getVersion() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<string>("lantern.app.getVersion");
}
