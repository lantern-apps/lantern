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

// src/updater/index.ts
var updater_exports = {};
__export(updater_exports, {
  check: () => check,
  launch: () => launch,
  offChecked: () => offChecked,
  offPrepared: () => offPrepared,
  onChecked: () => onChecked,
  onPrepared: () => onPrepared,
  perform: () => perform
});
module.exports = __toCommonJS(updater_exports);
var import_core2 = __toESM(require("@lantern-app/core"));

// src/utils.ts
var import_core = __toESM(require("@lantern-app/core"));
function throwErrorIfNoLantern() {
  if (!import_core.default) {
    throw new Error("lantern is not exists");
  }
}

// src/updater/index.ts
function launch() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.updater.launch");
}
function perform() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.updater.perform");
}
function check() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.updater.check");
}
function onPrepared() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.listen("lantern.updater.onPrepared");
}
function offPrepared() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.unlisten("lantern.updater.onPrepared");
}
function onChecked() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.listen("lantern.updater.onChecked");
}
function offChecked() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.listen("lantern.updater.onChecked");
}
// Annotate the CommonJS export names for ESM import in node:
0 && (module.exports = {
  check,
  launch,
  offChecked,
  offPrepared,
  onChecked,
  onPrepared,
  perform
});
//# sourceMappingURL=index.js.map