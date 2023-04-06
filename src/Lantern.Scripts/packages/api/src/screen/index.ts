import lantern from "@lantern-app/core";

import { Rectangle } from "../models";
import { throwErrorIfNoLantern } from "../utils";

export function getAll() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<Screen[]>("lantern.screen.getAll");
}

export function getCurrent() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<Screen | null>("lantern.screen.getCurrent");
}

export interface Screen {
  scaleFactor: number;
  bounds: Rectangle;
  workingArea: Rectangle;
  primary: boolean;
}
