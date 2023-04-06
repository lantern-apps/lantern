import lantern from "@lantern-app/core";

import { throwErrorIfNoLantern } from "../utils";

export function launch() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.updater.launch");
}

export function perform() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.updater.perform");
}

export function check() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<UpdateInfo>("lantern.updater.check");
}

export function onPrepared() {
  throwErrorIfNoLantern();

  return lantern.ipc.listen<PreparedEvent>("lantern.updater.onPrepared");
}

export function offPrepared() {
  throwErrorIfNoLantern();

  return lantern.ipc.unlisten("lantern.updater.onPrepared");
}

export function onChecked() {
  throwErrorIfNoLantern();

  return lantern.ipc.listen<CheckedEvent>("lantern.updater.onChecked");
}

export function offChecked() {
  throwErrorIfNoLantern();

  return lantern.ipc.listen("lantern.updater.onChecked");
}

export interface UpdateInfo {
  version: string;
  size: string;
}

export interface PreparedEvent {
  version: string;
  size: string;
}

export interface CheckedEvent {
  version: string;
  size: string;
}
