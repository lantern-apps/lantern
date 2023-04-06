import lantern from "@lantern-app/core";

import { ResizedEvent, MovedEvent } from "./events";
import { Position, Size, WindowTitleBarStyle } from "../models";
import { throwErrorIfNoLantern } from "../utils";

export function restore(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.restore", null, window);
}

export function maximize(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.maximize", null, window);
}

export function minimize(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.minimize", null, window);
}

export function fullScreen(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.fullScreen", null, window);
}

export function close(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.close", null, window);
}

export function hide(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.hide", null, window);
}

export function show(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.show", null, window);
}

export function activate(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.activate", null, window);
}

export function center(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.center", null, window);
}

export function setAlwaysOnTop(alwaysOnTop: boolean, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke(
    "lantern.window.setAlwaysOnTop",
    alwaysOnTop,
    window
  );
}

export function setSize(width: number, height: number, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke(
    "lantern.window.setSize",
    {
      width,
      height,
    },
    window
  );
}

export function setPosition(x: number, y: number, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke(
    "lantern.window.setPosition",
    {
      x,
      y,
    },
    window
  );
}

export function setTitle(title: string, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.setTitle", title, window);
}

export function setUrl(url: string, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.setUrl", url, window);
}

export function setMinSize(width: number, height: number, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke(
    "lantern.window.setMinSize",
    {
      width,
      height,
    },
    window
  );
}

export function setMaxSize(width: number, height: number, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke(
    "lantern.window.setMaxSize",
    {
      width,
      height,
    },
    window
  );
}

export function setSkipTaskbar(skipTaskbar: boolean, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke(
    "lantern.window.setSkipTaskbar",
    skipTaskbar,
    window
  );
}

export function setResizable(resizable: boolean, window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke("lantern.window.setResizable", resizable, window);
}

export function setTitleBarStyle(
  titleBarStyle: WindowTitleBarStyle,
  window?: string
) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke(
    "lantern.window.setTitleBarStyle",
    titleBarStyle,
    window
  );
}

export function isMaximized(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>(
    "lantern.window.isMaximized",
    null,
    window
  );
}

export function isMinimized(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>(
    "lantern.window.isMinimized",
    null,
    window
  );
}

export function isFullScreen(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>(
    "lantern.window.isFullScreen",
    null,
    window
  );
}

export function isAlwaysOnTop(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>(
    "lantern.window.isAlwaysOnTop",
    null,
    window
  );
}

export function isVisible(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>("lantern.window.isVisible", null, window);
}

export function isActive(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>("lantern.window.isActive", null, window);
}

export function isSkipTaskbar(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>(
    "lantern.window.isSkipTaskbar",
    null,
    window
  );
}

export function isResizable(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<boolean>(
    "lantern.window.isResizable",
    null,
    window
  );
}

export function getTitle(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<string | null>(
    "lantern.window.getTitle",
    null,
    window
  );
}

export function getUrl(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<string | null>(
    "lantern.window.getUrl",
    null,
    window
  );
}

export function getSize(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<Size>("lantern.window.getSize", null, window);
}

export function getPosition(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<Position>(
    "lantern.window.getPosition",
    null,
    window
  );
}

export function getMinSize(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<Size>("lantern.window.getMinSize", null, window);
}

export function getMaxSize(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<Size>("lantern.window.getMaxSize", null, window);
}

export function getTitleBarStyle(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.invoke<WindowTitleBarStyle>(
    "lantern.window.getTitleBarStyle",
    null,
    window
  );
}

export function onClosing(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.listen("lantern.window.onClosing", window);
}

export function offClosing(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.unlisten("lantern.window.onClosing", window);
}

export function onClosed(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.listen("lantern.window.onClosed", window);
}

export function offClosed(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.unlisten("lantern.window.onClosed", window);
}

export function onResized(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.listen<ResizedEvent>("lantern.window.onResized", window);
}

export function offResized(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.unlisten("lantern.window.onResized", window);
}

export function onMoved(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.listen<MovedEvent>("lantern.window.onMoved", window);
}

export function offMoved(window?: string) {
  throwErrorIfNoLantern();

  return lantern.ipc.unlisten("lantern.window.onMoved", window);
}
