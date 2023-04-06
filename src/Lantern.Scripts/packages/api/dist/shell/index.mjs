import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/shell/index.ts
import lantern from "@lantern-app/core";
function open(path) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.shell.open", {
    path
  });
}
export {
  open
};
//# sourceMappingURL=index.mjs.map