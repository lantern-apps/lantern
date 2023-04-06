// src/utils.ts
import lantern from "@lantern-app/core";
function throwErrorIfNoLantern() {
  if (!lantern) {
    throw new Error("lantern is not exists");
  }
}

export {
  throwErrorIfNoLantern
};
//# sourceMappingURL=chunk-SX5Y3YHK.mjs.map