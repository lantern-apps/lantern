import lantern from "@lantern-app/core";

import { throwErrorIfNoLantern } from "../utils";

export function open(path: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.shell.open", {
    path,
  });
}
