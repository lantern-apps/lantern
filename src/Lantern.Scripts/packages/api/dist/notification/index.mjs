import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/notification/index.ts
import lantern from "@lantern-app/core";
function send(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.notification.send", options);
}
export {
  send
};
//# sourceMappingURL=index.mjs.map