import {
  throwErrorIfNoLantern
} from "../chunk-SX5Y3YHK.mjs";

// src/window/actions.ts
import lantern from "@lantern-app/core";
function restore(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.restore", null, window);
}
function maximize(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.maximize", null, window);
}
function minimize(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.minimize", null, window);
}
function fullScreen(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.fullScreen", null, window);
}
function close(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.close", null, window);
}
function hide(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.hide", null, window);
}
function show(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.show", null, window);
}
function activate(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.activate", null, window);
}
function center(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.center", null, window);
}
function setAlwaysOnTop(alwaysOnTop, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.setAlwaysOnTop",
    alwaysOnTop,
    window
  );
}
function setSize(width, height, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.setSize",
    {
      width,
      height
    },
    window
  );
}
function setPosition(x, y, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.setPosition",
    {
      x,
      y
    },
    window
  );
}
function setTitle(title, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.setTitle", title, window);
}
function setUrl(url, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.setUrl", url, window);
}
function setMinSize(width, height, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.setMinSize",
    {
      width,
      height
    },
    window
  );
}
function setMaxSize(width, height, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.setMaxSize",
    {
      width,
      height
    },
    window
  );
}
function setSkipTaskbar(skipTaskbar, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.setSkipTaskbar",
    skipTaskbar,
    window
  );
}
function setResizable(resizable, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.setResizable", resizable, window);
}
function setTitleBarStyle(titleBarStyle, window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.setTitleBarStyle",
    titleBarStyle,
    window
  );
}
function isMaximized(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.isMaximized",
    null,
    window
  );
}
function isMinimized(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.isMinimized",
    null,
    window
  );
}
function isFullScreen(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.isFullScreen",
    null,
    window
  );
}
function isAlwaysOnTop(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.isAlwaysOnTop",
    null,
    window
  );
}
function isVisible(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.isVisible", null, window);
}
function isActive(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.isActive", null, window);
}
function isSkipTaskbar(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.isSkipTaskbar",
    null,
    window
  );
}
function isResizable(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.isResizable",
    null,
    window
  );
}
function getTitle(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.getTitle",
    null,
    window
  );
}
function getUrl(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.getUrl",
    null,
    window
  );
}
function getSize(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.getSize", null, window);
}
function getPosition(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.getPosition",
    null,
    window
  );
}
function getMinSize(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.getMinSize", null, window);
}
function getMaxSize(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.window.getMaxSize", null, window);
}
function getTitleBarStyle(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke(
    "lantern.window.getTitleBarStyle",
    null,
    window
  );
}
function onClosing(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.window.onClosing", window);
}
function offClosing(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.window.onClosing", window);
}
function onClosed(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.window.onClosed", window);
}
function offClosed(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.window.onClosed", window);
}
function onResized(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.window.onResized", window);
}
function offResized(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.window.onResized", window);
}
function onMoved(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.window.onMoved", window);
}
function offMoved(window) {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.window.onMoved", window);
}

// src/window/functions.ts
import lantern2 from "@lantern-app/core";

// src/window/window.ts
var Window = class {
  #name;
  get name() {
    return this.#name;
  }
  constructor(name) {
    this.#name = name || void 0;
  }
  restore() {
    return restore(this.name);
  }
  maximize() {
    return maximize(this.name);
  }
  minimize() {
    return minimize(this.name);
  }
  fullScreen() {
    return fullScreen(this.name);
  }
  close() {
    return close(this.name);
  }
  hide() {
    return hide(this.name);
  }
  show() {
    return show(this.name);
  }
  activate() {
    return activate(this.name);
  }
  center() {
    return center(this.name);
  }
  setAlwaysOnTop(alwaysOnTop) {
    return setAlwaysOnTop(alwaysOnTop, this.name);
  }
  setSize(width, height) {
    return setSize(width, height, this.name);
  }
  setPosition(x, y) {
    return setPosition(x, y, this.name);
  }
  setTitle(title) {
    return setTitle(title, this.name);
  }
  setUrl(url) {
    return setUrl(url, this.name);
  }
  setMinSize(width, height) {
    return setMinSize(width, height, this.name);
  }
  setMaxSize(width, height) {
    return setMaxSize(width, height, this.name);
  }
  setSkipTaskbar(skipTaskbar) {
    return setSkipTaskbar(skipTaskbar, this.name);
  }
  setResizable(resizable) {
    return setResizable(resizable, this.name);
  }
  setTitleBarStyle(titleBarStyle) {
    return setTitleBarStyle(titleBarStyle, this.name);
  }
  isMaximized() {
    return isMaximized(this.name);
  }
  isMinimized() {
    return isMinimized(this.name);
  }
  isFullScreen() {
    return isFullScreen(this.name);
  }
  isAlwaysOnTop() {
    return isAlwaysOnTop(this.name);
  }
  isVisible() {
    return isVisible(this.name);
  }
  isActive() {
    return isActive(this.name);
  }
  isSkipTaskbar() {
    return isSkipTaskbar(this.name);
  }
  isResizable() {
    return isResizable(this.name);
  }
  getTitle() {
    return getTitle(this.name);
  }
  getUrl() {
    return getUrl(this.name);
  }
  getSize() {
    return getSize(this.name);
  }
  getPosition() {
    return getPosition(this.name);
  }
  getMinSize() {
    return getMinSize(this.name);
  }
  getMaxSize() {
    return getMaxSize(this.name);
  }
  getTitleBarStyle() {
    return getTitleBarStyle(this.name);
  }
  onClosing() {
    return onClosing(this.name);
  }
  offClosing() {
    return offClosing(this.name);
  }
  onClosed() {
    return onClosed(this.name);
  }
  offClosed() {
    return offClosed(this.name);
  }
  onResized() {
    return onResized(this.name);
  }
  offResized() {
    return offResized(this.name);
  }
  onMoved() {
    return onMoved(this.name);
  }
  offMoved() {
    return offMoved(this.name);
  }
};

// src/window/functions.ts
async function getAll() {
  throwErrorIfNoLantern();
  const names = await lantern2.ipc.invoke("lantern.window.getAll");
  return names.map((name) => new Window(name));
}
async function getCurrent() {
  throwErrorIfNoLantern();
  return new Window();
}
async function create(options) {
  throwErrorIfNoLantern();
  await lantern2.ipc.invoke("lantern.window.create", options);
  return new Window(options.name);
}
export {
  Window,
  activate,
  center,
  close,
  create,
  fullScreen,
  getAll,
  getCurrent,
  getMaxSize,
  getMinSize,
  getPosition,
  getSize,
  getTitle,
  getTitleBarStyle,
  getUrl,
  hide,
  isActive,
  isAlwaysOnTop,
  isFullScreen,
  isMaximized,
  isMinimized,
  isResizable,
  isSkipTaskbar,
  isVisible,
  maximize,
  minimize,
  offClosed,
  offClosing,
  offMoved,
  offResized,
  onClosed,
  onClosing,
  onMoved,
  onResized,
  restore,
  setAlwaysOnTop,
  setMaxSize,
  setMinSize,
  setPosition,
  setResizable,
  setSize,
  setSkipTaskbar,
  setTitle,
  setTitleBarStyle,
  setUrl,
  show
};
//# sourceMappingURL=index.mjs.map