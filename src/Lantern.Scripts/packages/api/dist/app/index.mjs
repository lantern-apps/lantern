import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/app/index.ts
import lantern from "@lantern-app/core";
function shutdown() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.app.shutdown");
}
function getVersion() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.app.getVersion");
}
export {
  getVersion,
  shutdown
};
//# sourceMappingURL=index.mjs.map