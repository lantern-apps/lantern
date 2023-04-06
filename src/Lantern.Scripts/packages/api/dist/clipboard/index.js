"use strict";
var __create = Object.create;
var __defProp = Object.defineProperty;
var __getOwnPropDesc = Object.getOwnPropertyDescriptor;
var __getOwnPropNames = Object.getOwnPropertyNames;
var __getProtoOf = Object.getPrototypeOf;
var __hasOwnProp = Object.prototype.hasOwnProperty;
var __export = (target, all) => {
  for (var name in all)
    __defProp(target, name, { get: all[name], enumerable: true });
};
var __copyProps = (to, from, except, desc) => {
  if (from && typeof from === "object" || typeof from === "function") {
    for (let key of __getOwnPropNames(from))
      if (!__hasOwnProp.call(to, key) && key !== except)
        __defProp(to, key, { get: () => from[key], enumerable: !(desc = __getOwnPropDesc(from, key)) || desc.enumerable });
  }
  return to;
};
var __toESM = (mod, isNodeMode, target) => (target = mod != null ? __create(__getProtoOf(mod)) : {}, __copyProps(
  // If the importer is in node compatibility mode or this is not an ESM
  // file that has been converted to a CommonJS file using a Babel-
  // compatible transform (i.e. "__esModule" has not been set), then set
  // "default" to the CommonJS "module.exports" for node compatibility.
  isNodeMode || !mod || !mod.__esModule ? __defProp(target, "default", { value: mod, enumerable: true }) : target,
  mod
));
var __toCommonJS = (mod) => __copyProps(__defProp({}, "__esModule", { value: true }), mod);

// src/clipboard/index.ts
var clipboard_exports = {};
__export(clipboard_exports, {
  getText: () => getText,
  setText: () => setText
});
module.exports = __toCommonJS(clipboard_exports);
var import_core2 = __toESM(require("@lantern-app/core"));

// src/utils.ts
var import_core = __toESM(require("@lantern-app/core"));
function throwErrorIfNoLantern() {
  if (!import_core.default) {
    throw new Error("lantern is not exists");
  }
}

// src/clipboard/index.ts
function setText(text) {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.clipboard.setText", text);
}
function getText() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.clipboard.getText");
}
// Annotate the CommonJS export names for ESM import in node:
0 && (module.exports = {
  getText,
  setText
});
//# sourceMappingURL=index.js.map