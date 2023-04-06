import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/screen/index.ts
import lantern from "@lantern-app/core";
function getAll() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.screen.getAll");
}
function getCurrent() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.screen.getCurrent");
}
export {
  getAll,
  getCurrent
};
//# sourceMappingURL=index.mjs.map