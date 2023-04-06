import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/updater/index.ts
import lantern from "@lantern-app/core";
function launch() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.updater.launch");
}
function perform() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.updater.perform");
}
function check() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.updater.check");
}
function onPrepared() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.updater.onPrepared");
}
function offPrepared() {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.updater.onPrepared");
}
function onChecked() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.updater.onChecked");
}
function offChecked() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.updater.onChecked");
}
export {
  check,
  launch,
  offChecked,
  offPrepared,
  onChecked,
  onPrepared,
  perform
};
//# sourceMappingURL=index.mjs.map