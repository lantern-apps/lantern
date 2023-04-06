import lantern from "@lantern-app/core";

import { throwErrorIfNoLantern } from "../utils";

export function message(options: MessageOptions) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.dialog.message", options);
}

export function ask(options: AskOptions) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>("lantern.dialog.ask", options);
}

export function confirm(options: ConfirmOptions) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>("lantern.dialog.confirm", options);
}

export function openFile(options: OpenFileOptions) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<string[]>("lantern.dialog.openFile", options);
}

export function openFolder(options: OpenFolderOptions) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<string[]>("lantern.dialog.openFolder", options);
}

export function saveFile(options: SaveFileOptions) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<string | null>("lantern.dialog.saveFile", options);
}

export interface MessageOptions {
  title?: string;
  body?: string;
}

export interface AskOptions {
  title?: string;
  body?: string;
}

export interface ConfirmOptions {
  title?: string;
  body?: string;
}

export interface OpenFileOptions {
  title?: string;
  defaultPath?: string;
  filters?: Filter[];
  multiple?: boolean;
}

export interface OpenFolderOptions {
  title?: string;
  defaultPath?: string;
  multiple?: boolean;
}

export interface SaveFileOptions {
  title?: string;
  defaultPath?: string;
  filters?: Filter[];
}

export interface Filter {
  name?: string;
  extensions?: string[];
}
