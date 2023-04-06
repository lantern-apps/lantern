import lantern from "@lantern-app/core";

import { throwErrorIfNoLantern } from "../utils";

export function setText(text: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.clipboard.setText", text);
}

export function getText() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<string>("lantern.clipboard.getText");
}
