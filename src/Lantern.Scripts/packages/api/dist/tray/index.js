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

// src/tray/index.ts
var tray_exports = {};
__export(tray_exports, {
  hide: () => hide,
  isVisible: () => isVisible,
  offClick: () => offClick,
  offMenuItemClick: () => offMenuItemClick,
  onClick: () => onClick,
  onMenuItemClick: () => onMenuItemClick,
  setMenu: () => setMenu,
  setTooltip: () => setTooltip,
  show: () => show
});
module.exports = __toCommonJS(tray_exports);
var import_core2 = __toESM(require("@lantern-app/core"));

// src/utils.ts
var import_core = __toESM(require("@lantern-app/core"));
function throwErrorIfNoLantern() {
  if (!import_core.default) {
    throw new Error("lantern is not exists");
  }
}

// src/tray/index.ts
function show() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.tray.show");
}
function hide() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.tray.hide");
}
function isVisible() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.tray.isVisible");
}
function setTooltip(tooltip) {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.tray.setTooltip", tooltip);
}
function setMenu(items) {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.invoke("lantern.tray.setMenu", {
    items
  });
}
function onClick() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.listen("lantern.tray.onClick");
}
function offClick() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.unlisten("lantern.tray.onClick");
}
function onMenuItemClick() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.listen("lantern.tray.onMenuItemClick");
}
function offMenuItemClick() {
  throwErrorIfNoLantern();
  return import_core2.default.ipc.unlisten("lantern.tray.onMenuItemClick");
}
// Annotate the CommonJS export names for ESM import in node:
0 && (module.exports = {
  hide,
  isVisible,
  offClick,
  offMenuItemClick,
  onClick,
  onMenuItemClick,
  setMenu,
  setTooltip,
  show
});
//# sourceMappingURL=index.js.map