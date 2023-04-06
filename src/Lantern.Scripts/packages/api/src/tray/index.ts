import lantern from "@lantern-app/core";

import { throwErrorIfNoLantern } from "../utils";

export function show() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.tray.show");
}

export function hide() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.tray.hide");
}

export function isVisible() {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>("lantern.tray.isVisible");
}

export function setTooltip(tooltip: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.tray.setTooltip", tooltip);
}

export function setMenu(items: MenuItem[]) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.tray.setMenu", {
    items,
  });
}

export function onClick() {
  throwErrorIfNoLantern();

  return lantern.ipc.listen("lantern.tray.onClick");
}

export function offClick() {
  throwErrorIfNoLantern();

  return lantern.ipc.unlisten("lantern.tray.onClick");
}

export function onMenuItemClick() {
  throwErrorIfNoLantern();

  return lantern.ipc.listen<MenuItemClickEvent>("lantern.tray.onMenuItemClick");
}

export function offMenuItemClick() {
  throwErrorIfNoLantern();

  return lantern.ipc.unlisten("lantern.tray.onMenuItemClick");
}

export interface MenuNormalItem {
  type?: "normal";
  id: string;
  text: string;
  enabled?: boolean;
}

export interface MenuSubmenuItem {
  type: "submenu";
  text: string;
  enabled?: boolean;
  items?: MenuItem[];
}

export interface MenuSeparatorItem {
  type: "separator";
}

export type MenuItem = MenuNormalItem | MenuSubmenuItem | MenuSeparatorItem;

export interface MenuItemClickEvent {
  id: string;
}
