import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/clipboard/index.ts
import lantern from "@lantern-app/core";
function setText(text) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.clipboard.setText", text);
}
function getText() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.clipboard.getText");
}
export {
  getText,
  setText
};
//# sourceMappingURL=index.mjs.map