import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/tray/index.ts
import lantern from "@lantern-app/core";
function show() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.tray.show");
}
function hide() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.tray.hide");
}
function isVisible() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.tray.isVisible");
}
function setTooltip(tooltip) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.tray.setTooltip", tooltip);
}
function setMenu(items) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.tray.setMenu", {
    items
  });
}
function onClick() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.tray.onClick");
}
function offClick() {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.tray.onClick");
}
function onMenuItemClick() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.tray.onMenuItemClick");
}
function offMenuItemClick() {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.tray.onMenuItemClick");
}
export {
  hide,
  isVisible,
  offClick,
  offMenuItemClick,
  onClick,
  onMenuItemClick,
  setMenu,
  setTooltip,
  show
};
//# sourceMappingURL=index.mjs.map