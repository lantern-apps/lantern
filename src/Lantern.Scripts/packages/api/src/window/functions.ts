import lantern from "@lantern-app/core";

import { CreateOptions } from "./options";
import { Window } from "./window";
import { throwErrorIfNoLantern } from "../utils";

export async function getAll() {
  throwErrorIfNoLantern();

  const names = await lantern.ipc.invoke<string[]>("lantern.window.getAll");

  return names.map((name) => new Window(name));
}

export async function getCurrent() {
  throwErrorIfNoLantern();

  return new Window();
}

export async function create(options: CreateOptions) {
  throwErrorIfNoLantern();

  await lantern.ipc.invoke("lantern.window.create", options);

  return new Window(options.name);
}
