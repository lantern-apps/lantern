import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/dialog/index.ts
import lantern from "@lantern-app/core";
function message(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.message", options);
}
function ask(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.ask", options);
}
function confirm(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.confirm", options);
}
function openFile(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.openFile", options);
}
function openFolder(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.openFolder", options);
}
function saveFile(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.saveFile", options);
}
export {
  ask,
  confirm,
  message,
  openFile,
  openFolder,
  saveFile
};
//# sourceMappingURL=index.mjs.map