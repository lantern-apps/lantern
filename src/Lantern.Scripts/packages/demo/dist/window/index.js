var __accessCheck = (obj, member, msg) => {
  if (!member.has(obj))
    throw TypeError("Cannot " + msg);
};
var __privateGet = (obj, member, getter) => {
  __accessCheck(obj, member, "read from private field");
  return getter ? getter.call(obj) : member.get(obj);
};
var __privateAdd = (obj, member, value) => {
  if (member.has(obj))
    throw TypeError("Cannot add the same private member more than once");
  member instanceof WeakSet ? member.add(obj) : member.set(obj, value);
};
var __privateSet = (obj, member, value, setter) => {
  __accessCheck(obj, member, "write to private field");
  setter ? setter.call(obj, value) : member.set(obj, value);
  return value;
};
var _name, _a;
import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
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
var Window = (_a = class {
  constructor(name) {
    __privateAdd(this, _name, void 0);
    __privateSet(this, _name, name || void 0);
  }
  get name() {
    return __privateGet(this, _name);
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
}, _name = new WeakMap(), _a);
async function getAll() {
  throwErrorIfNoLantern();
  const names = await lantern.ipc.invoke("lantern.window.getAll");
  return names.map((name) => new Window(name));
}
async function create(options) {
  throwErrorIfNoLantern();
  await lantern.ipc.invoke("lantern.window.create", options);
  return new Window(options.name);
}
function createOptionElement(value, text) {
  const option = document.createElement("option");
  option.value = value || "";
  option.text = text || "";
  return option;
}
const windowSelectWindowSelect = document.querySelector(
  "#window-select-window-select"
);
const windowSelectRefreshButton = document.querySelector(
  "#window-select-refresh-button"
);
const windowSelectRefreshResult = document.querySelector(
  "#window-select-refresh-result"
);
windowSelectRefreshButton.addEventListener("click", async () => {
  windowSelectRefreshResult.textContent = "";
  try {
    const result = await getAll();
    windowSelectRefreshResult.textContent = `success`;
    windowSelectWindowSelect.replaceChildren(
      createOptionElement("", "current"),
      ...result.map((window) => createOptionElement(window.name, window.name))
    );
  } catch (error) {
    windowSelectRefreshResult.textContent = `error: ${error.message}`;
  }
});
function getSelectedWindow() {
  return new Window(windowSelectWindowSelect.value);
}
const windowActivateInvokeButton = document.querySelector(
  "#window-activate-invoke-button"
);
const windowActivateInvokeResult = document.querySelector(
  "#window-activate-invoke-result"
);
windowActivateInvokeButton.addEventListener("click", async () => {
  windowActivateInvokeResult.textContent = "";
  try {
    await getSelectedWindow().activate();
    windowActivateInvokeResult.textContent = `success`;
  } catch (error) {
    windowActivateInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowCenterInvokeButton = document.querySelector(
  "#window-center-invoke-button"
);
const windowCenterInvokeResult = document.querySelector(
  "#window-center-invoke-result"
);
windowCenterInvokeButton.addEventListener("click", async () => {
  windowCenterInvokeResult.textContent = "";
  try {
    await getSelectedWindow().center();
    windowCenterInvokeResult.textContent = `success`;
  } catch (error) {
    windowCenterInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowCloseInvokeButton = document.querySelector(
  "#window-close-invoke-button"
);
const windowCloseInvokeResult = document.querySelector(
  "#window-close-invoke-result"
);
windowCloseInvokeButton.addEventListener("click", async () => {
  windowCloseInvokeResult.textContent = "";
  try {
    await getSelectedWindow().close();
    windowCloseInvokeResult.textContent = `success`;
  } catch (error) {
    windowCloseInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowCreateNameInput = document.querySelector(
  "#window-create-name-input"
);
const windowCreateTitleInput = document.querySelector(
  "#window-create-title-input"
);
const windowCreateUrlInput = document.querySelector(
  "#window-create-url-input"
);
const windowCreateWidthInput = document.querySelector(
  "#window-create-width-input"
);
const windowCreateHeightInput = document.querySelector(
  "#window-create-height-input"
);
const windowCreateXInput = document.querySelector(
  "#window-create-x-input"
);
const windowCreateYInput = document.querySelector(
  "#window-create-y-input"
);
const windowCreateMinWidthInput = document.querySelector(
  "#window-create-min-width-input"
);
const windowCreateMinHeightInput = document.querySelector(
  "#window-create-min-height-input"
);
const windowCreateMaxWidthInput = document.querySelector(
  "#window-create-max-width-input"
);
const windowCreateMaxHeightInput = document.querySelector(
  "#window-create-max-height-input"
);
const windowCreateInvokeButton = document.querySelector(
  "#window-create-invoke-button"
);
const windowCreateInvokeResult = document.querySelector(
  "#window-create-invoke-result"
);
const windowCreateVisibleInput = document.querySelector(
  "#window-create-visible-input"
);
const windowCreateCenterInput = document.querySelector(
  "#window-create-center-input"
);
const windowCreateAlwaysOnTopInput = document.querySelector(
  "#window-create-always-on-top-input"
);
const windowCreateSkipTaskbarInput = document.querySelector(
  "#window-create-skip-taskbar-input"
);
const windowCreateResizableInput = document.querySelector(
  "#window-create-resizable-input"
);
const windowCreateTitleBarStyleSelect = document.querySelector(
  "#window-create-title-bar-style-select"
);
windowCreateInvokeButton.addEventListener("click", async () => {
  const name = windowCreateNameInput.value;
  const title = windowCreateTitleInput.value;
  const url = windowCreateUrlInput.value;
  const width = windowCreateWidthInput.valueAsNumber;
  const height = windowCreateHeightInput.valueAsNumber;
  const x = windowCreateXInput.valueAsNumber;
  const y = windowCreateYInput.valueAsNumber;
  const minWidth = windowCreateMinWidthInput.valueAsNumber;
  const minHeight = windowCreateMinHeightInput.valueAsNumber;
  const maxWidth = windowCreateMaxWidthInput.valueAsNumber;
  const maxHeight = windowCreateMaxHeightInput.valueAsNumber;
  const visible = windowCreateVisibleInput.checked;
  const center2 = windowCreateCenterInput.checked;
  const alwaysOnTop = windowCreateAlwaysOnTopInput.checked;
  const skipTaskbar = windowCreateSkipTaskbarInput.checked;
  const resizable = windowCreateResizableInput.checked;
  const titleBarStyle = windowCreateTitleBarStyleSelect.value;
  windowCreateInvokeResult.textContent = "";
  try {
    await create({
      name,
      title,
      url,
      width,
      height,
      x,
      y,
      minWidth,
      minHeight,
      maxWidth,
      maxHeight,
      visible,
      center: center2,
      alwaysOnTop,
      skipTaskbar,
      resizable,
      titleBarStyle
    });
    windowCreateInvokeResult.textContent = `success`;
  } catch (error) {
    windowCreateInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowFullScreenInvokeButton = document.querySelector(
  "#window-full-screen-invoke-button"
);
const windowFullScreenInvokeResult = document.querySelector(
  "#window-full-screen-invoke-result"
);
windowFullScreenInvokeButton.addEventListener("click", async () => {
  windowFullScreenInvokeResult.textContent = "";
  try {
    await getSelectedWindow().fullScreen();
    windowFullScreenInvokeResult.textContent = `success`;
  } catch (error) {
    windowFullScreenInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowGetMaxSizeInvokeButton = document.querySelector(
  "#window-get-max-size-invoke-button"
);
const windowGetMaxSizeInvokeResult = document.querySelector(
  "#window-get-max-size-invoke-result"
);
windowGetMaxSizeInvokeButton.addEventListener("click", async () => {
  windowGetMaxSizeInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getMaxSize();
    windowGetMaxSizeInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowGetMaxSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowGetMinSizeInvokeButton = document.querySelector(
  "#window-get-min-size-invoke-button"
);
const windowGetMinSizeInvokeResult = document.querySelector(
  "#window-get-min-size-invoke-result"
);
windowGetMinSizeInvokeButton.addEventListener("click", async () => {
  windowGetMinSizeInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getMinSize();
    windowGetMinSizeInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowGetMinSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowGetPositionInvokeButton = document.querySelector(
  "#window-get-position-invoke-button"
);
const windowGetPositionInvokeResult = document.querySelector(
  "#window-get-position-invoke-result"
);
windowGetPositionInvokeButton.addEventListener("click", async () => {
  windowGetPositionInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getPosition();
    windowGetPositionInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowGetPositionInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowGetSizeInvokeButton = document.querySelector(
  "#window-get-size-invoke-button"
);
const windowGetSizeInvokeResult = document.querySelector(
  "#window-get-size-invoke-result"
);
windowGetSizeInvokeButton.addEventListener("click", async () => {
  windowGetSizeInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getSize();
    windowGetSizeInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowGetSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowGetTitleBarStyleInvokeButton = document.querySelector(
  "#window-get-title-bar-style-invoke-button"
);
const windowGetTitleBarStyleInvokeResult = document.querySelector(
  "#window-get-title-bar-style-invoke-result"
);
windowGetTitleBarStyleInvokeButton.addEventListener("click", async () => {
  windowGetTitleBarStyleInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getTitleBarStyle();
    windowGetTitleBarStyleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowGetTitleBarStyleInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowGetTitleInvokeButton = document.querySelector(
  "#window-get-title-invoke-button"
);
const windowGetTitleInvokeResult = document.querySelector(
  "#window-get-title-invoke-result"
);
windowGetTitleInvokeButton.addEventListener("click", async () => {
  windowGetTitleInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getTitle();
    windowGetTitleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowGetTitleInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowGetUrlInvokeButton = document.querySelector(
  "#window-get-url-invoke-button"
);
const windowGetUrlInvokeResult = document.querySelector(
  "#window-get-url-invoke-result"
);
windowGetUrlInvokeButton.addEventListener("click", async () => {
  windowGetUrlInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getUrl();
    windowGetUrlInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error) {
    windowGetUrlInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowHideInvokeButton = document.querySelector(
  "#window-hide-invoke-button"
);
const windowHideInvokeResult = document.querySelector(
  "#window-hide-invoke-result"
);
windowHideInvokeButton.addEventListener("click", async () => {
  windowHideInvokeResult.textContent = "";
  try {
    await getSelectedWindow().hide();
    windowHideInvokeResult.textContent = `success`;
  } catch (error) {
    windowHideInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsActiveInvokeButton = document.querySelector(
  "#window-is-active-invoke-button"
);
const windowIsActiveInvokeResult = document.querySelector(
  "#window-is-active-invoke-result"
);
windowIsActiveInvokeButton.addEventListener("click", async () => {
  windowIsActiveInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isActive();
    windowIsActiveInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsActiveInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsAlwaysOnTopInvokeButton = document.querySelector(
  "#window-is-always-on-top-invoke-button"
);
const windowIsAlwaysOnTopInvokeResult = document.querySelector(
  "#window-is-always-on-top-invoke-result"
);
windowIsAlwaysOnTopInvokeButton.addEventListener("click", async () => {
  windowIsAlwaysOnTopInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isAlwaysOnTop();
    windowIsAlwaysOnTopInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsAlwaysOnTopInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsFullScreenInvokeButton = document.querySelector(
  "#window-is-full-screen-invoke-button"
);
const windowIsFullScreenInvokeResult = document.querySelector(
  "#window-is-full-screen-invoke-result"
);
windowIsFullScreenInvokeButton.addEventListener("click", async () => {
  windowIsFullScreenInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isFullScreen();
    windowIsFullScreenInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsFullScreenInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsMaximizedInvokeButton = document.querySelector(
  "#window-is-maximized-invoke-button"
);
const windowIsMaximizedInvokeResult = document.querySelector(
  "#window-is-maximized-invoke-result"
);
windowIsMaximizedInvokeButton.addEventListener("click", async () => {
  windowIsMaximizedInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isMaximized();
    windowIsMaximizedInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsMaximizedInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsMinimizedInvokeButton = document.querySelector(
  "#window-is-minimized-invoke-button"
);
const windowIsMinimizedInvokeResult = document.querySelector(
  "#window-is-minimized-invoke-result"
);
windowIsMinimizedInvokeButton.addEventListener("click", async () => {
  windowIsMinimizedInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isMinimized();
    windowIsMinimizedInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsMinimizedInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsResizableInvokeButton = document.querySelector(
  "#window-is-resizable-invoke-button"
);
const windowIsResizableInvokeResult = document.querySelector(
  "#window-is-resizable-invoke-result"
);
windowIsResizableInvokeButton.addEventListener("click", async () => {
  windowIsResizableInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isResizable();
    windowIsResizableInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsResizableInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsSkipTaskbarInvokeButton = document.querySelector(
  "#window-is-skip-taskbar-invoke-button"
);
const windowIsSkipTaskbarInvokeResult = document.querySelector(
  "#window-is-skip-taskbar-invoke-result"
);
windowIsSkipTaskbarInvokeButton.addEventListener("click", async () => {
  windowIsSkipTaskbarInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isSkipTaskbar();
    windowIsSkipTaskbarInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsSkipTaskbarInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowIsVisibleInvokeButton = document.querySelector(
  "#window-is-visible-invoke-button"
);
const windowIsVisibleInvokeResult = document.querySelector(
  "#window-is-visible-invoke-result"
);
windowIsVisibleInvokeButton.addEventListener("click", async () => {
  windowIsVisibleInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isVisible();
    windowIsVisibleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    windowIsVisibleInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowMaximizeInvokeButton = document.querySelector(
  "#window-maximize-invoke-button"
);
const windowMaximizeInvokeResult = document.querySelector(
  "#window-maximize-invoke-result"
);
windowMaximizeInvokeButton.addEventListener("click", async () => {
  windowMaximizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().maximize();
    windowMaximizeInvokeResult.textContent = `success`;
  } catch (error) {
    windowMaximizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowMinimizeInvokeButton = document.querySelector(
  "#window-minimize-invoke-button"
);
const windowMinimizeInvokeResult = document.querySelector(
  "#window-minimize-invoke-result"
);
windowMinimizeInvokeButton.addEventListener("click", async () => {
  windowMinimizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().minimize();
    windowMinimizeInvokeResult.textContent = `success`;
  } catch (error) {
    windowMinimizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowOnClosedListenButton = document.querySelector(
  "#window-on-closed-listen-button"
);
const windowOnClosedUnlistenButton = document.querySelector(
  "#window-on-closed-unlisten-button"
);
const windowOnClosedListenResult = document.querySelector(
  "#window-on-closed-listen-result"
);
let subscription$3;
windowOnClosedListenButton.addEventListener("click", async () => {
  if (subscription$3) {
    subscription$3.unsubscribe();
  }
  let times = 0;
  windowOnClosedListenResult.textContent = "";
  try {
    subscription$3 = getSelectedWindow().onClosed().subscribe(() => {
      windowOnClosedListenResult.textContent = `trigger ${++times} times`;
    });
  } catch (error) {
    windowOnClosedListenResult.textContent = `error: ${error.message}`;
  }
});
windowOnClosedUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offClosed();
  } catch (error) {
    windowOnClosedListenResult.textContent = `error: ${error.message}`;
  }
});
const windowOnClosingListenButton = document.querySelector(
  "#window-on-closing-listen-button"
);
const windowOnClosingUnlistenButton = document.querySelector(
  "#window-on-closing-unlisten-button"
);
const windowOnClosingListenResult = document.querySelector(
  "#window-on-closing-listen-result"
);
let subscription$2;
windowOnClosingListenButton.addEventListener("click", async () => {
  if (subscription$2) {
    subscription$2.unsubscribe();
  }
  let times = 0;
  windowOnClosingListenResult.textContent = "";
  try {
    subscription$2 = getSelectedWindow().onClosing().subscribe(() => {
      windowOnClosingListenResult.textContent = `trigger ${++times} times`;
    });
  } catch (error) {
    windowOnClosingListenResult.textContent = `error: ${error.message}`;
  }
});
windowOnClosingUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offClosing();
  } catch (error) {
    windowOnClosingListenResult.textContent = `error: ${error.message}`;
  }
});
const windowOnMovedListenButton = document.querySelector(
  "#window-on-moved-listen-button"
);
const windowOnMovedUnlistenButton = document.querySelector(
  "#window-on-moved-unlisten-button"
);
const windowOnMovedListenResult = document.querySelector(
  "#window-on-moved-listen-result"
);
let subscription$1;
windowOnMovedListenButton.addEventListener("click", async () => {
  if (subscription$1) {
    subscription$1.unsubscribe();
  }
  let times = 0;
  windowOnMovedListenResult.textContent = "";
  try {
    subscription$1 = getSelectedWindow().onMoved().subscribe((event) => {
      windowOnMovedListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error) {
    windowOnMovedListenResult.textContent = `error: ${error.message}`;
  }
});
windowOnMovedUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offMoved();
  } catch (error) {
    windowOnMovedListenResult.textContent = `error: ${error.message}`;
  }
});
const windowOnResizedListenButton = document.querySelector(
  "#window-on-resized-listen-button"
);
const windowOnResizedUnlistenButton = document.querySelector(
  "#window-on-resized-unlisten-button"
);
const windowOnResizedListenResult = document.querySelector(
  "#window-on-resized-listen-result"
);
let subscription;
windowOnResizedListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }
  let times = 0;
  windowOnResizedListenResult.textContent = "";
  try {
    subscription = getSelectedWindow().onResized().subscribe((event) => {
      windowOnResizedListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error) {
    windowOnResizedListenResult.textContent = `error: ${error.message}`;
  }
});
windowOnResizedUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offResized();
  } catch (error) {
    windowOnResizedListenResult.textContent = `error: ${error.message}`;
  }
});
const windowRestoreInvokeButton = document.querySelector(
  "#window-restore-invoke-button"
);
const windowRestoreInvokeResult = document.querySelector(
  "#window-restore-invoke-result"
);
windowRestoreInvokeButton.addEventListener("click", async () => {
  windowRestoreInvokeResult.textContent = "";
  try {
    await getSelectedWindow().restore();
    windowRestoreInvokeResult.textContent = `success`;
  } catch (error) {
    windowRestoreInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetAlwaysOnTopAlwaysOnTopInput = document.querySelector(
  "#window-set-always-on-top-always-on-top-input"
);
const windowSetAlwaysOnTopInvokeButton = document.querySelector(
  "#window-set-always-on-top-invoke-button"
);
const windowSetAlwaysOnTopInvokeResult = document.querySelector(
  "#window-set-always-on-top-invoke-result"
);
windowSetAlwaysOnTopInvokeButton.addEventListener("click", async () => {
  const alwaysOnTop = windowSetAlwaysOnTopAlwaysOnTopInput.checked;
  windowSetAlwaysOnTopInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setAlwaysOnTop(alwaysOnTop);
    windowSetAlwaysOnTopInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetAlwaysOnTopInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetMaxSizeWidthInput = document.querySelector(
  "#window-set-max-size-width-input"
);
const windowSetMaxSizeHeightInput = document.querySelector(
  "#window-set-max-size-height-input"
);
const windowSetMaxSizeInvokeButton = document.querySelector(
  "#window-set-max-size-invoke-button"
);
const windowSetMaxSizeInvokeResult = document.querySelector(
  "#window-set-max-size-invoke-result"
);
windowSetMaxSizeInvokeButton.addEventListener("click", async () => {
  const width = windowSetMaxSizeWidthInput.valueAsNumber;
  const height = windowSetMaxSizeHeightInput.valueAsNumber;
  windowSetMaxSizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setMaxSize(width, height);
    windowSetMaxSizeInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetMaxSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetMinSizeWidthInput = document.querySelector(
  "#window-set-min-size-width-input"
);
const windowSetMinSizeHeightInput = document.querySelector(
  "#window-set-min-size-height-input"
);
const windowSetMinSizeInvokeButton = document.querySelector(
  "#window-set-min-size-invoke-button"
);
const windowSetMinSizeInvokeResult = document.querySelector(
  "#window-set-min-size-invoke-result"
);
windowSetMinSizeInvokeButton.addEventListener("click", async () => {
  const width = windowSetMinSizeWidthInput.valueAsNumber;
  const height = windowSetMinSizeHeightInput.valueAsNumber;
  windowSetMinSizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setMinSize(width, height);
    windowSetMinSizeInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetMinSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetPositionXInput = document.querySelector(
  "#window-set-position-x-input"
);
const windowSetPositionYInput = document.querySelector(
  "#window-set-position-y-input"
);
const windowSetPositionInvokeButton = document.querySelector(
  "#window-set-position-invoke-button"
);
const windowSetPositionInvokeResult = document.querySelector(
  "#window-set-position-invoke-result"
);
windowSetPositionInvokeButton.addEventListener("click", async () => {
  const x = windowSetPositionXInput.valueAsNumber;
  const y = windowSetPositionYInput.valueAsNumber;
  windowSetPositionInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setPosition(x, y);
    windowSetPositionInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetPositionInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetResizableResizableInput = document.querySelector(
  "#window-set-resizable-resizable-input"
);
const windowSetResizableInvokeButton = document.querySelector(
  "#window-set-resizable-invoke-button"
);
const windowSetResizableInvokeResult = document.querySelector(
  "#window-set-resizable-invoke-result"
);
windowSetResizableInvokeButton.addEventListener("click", async () => {
  const resizable = windowSetResizableResizableInput.checked;
  windowSetResizableInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setResizable(resizable);
    windowSetResizableInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetResizableInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetSizeWidthInput = document.querySelector(
  "#window-set-size-width-input"
);
const windowSetSizeHeightInput = document.querySelector(
  "#window-set-size-height-input"
);
const windowSetSizeInvokeButton = document.querySelector(
  "#window-set-size-invoke-button"
);
const windowSetSizeInvokeResult = document.querySelector(
  "#window-set-size-invoke-result"
);
windowSetSizeInvokeButton.addEventListener("click", async () => {
  const width = windowSetSizeWidthInput.valueAsNumber;
  const height = windowSetSizeHeightInput.valueAsNumber;
  windowSetSizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setSize(width, height);
    windowSetSizeInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetSkipTaskbarSkipTaskbarInput = document.querySelector(
  "#window-set-skip-taskbar-skip-taskbar-input"
);
const windowSetSkipTaskbarInvokeButton = document.querySelector(
  "#window-set-skip-taskbar-invoke-button"
);
const windowSetSkipTaskbarInvokeResult = document.querySelector(
  "#window-set-skip-taskbar-invoke-result"
);
windowSetSkipTaskbarInvokeButton.addEventListener("click", async () => {
  const skipTaskbar = windowSetSkipTaskbarSkipTaskbarInput.checked;
  windowSetSkipTaskbarInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setSkipTaskbar(skipTaskbar);
    windowSetSkipTaskbarInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetSkipTaskbarInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetTitleBarStyleTitleBarStyleSelect = document.querySelector(
  "#window-set-title-bar-style-title-bar-style-select"
);
const windowSetTitleBarStyleInvokeButton = document.querySelector(
  "#window-set-title-bar-style-invoke-button"
);
const windowSetTitleBarStyleInvokeResult = document.querySelector(
  "#window-set-title-bar-style-invoke-result"
);
windowSetTitleBarStyleInvokeButton.addEventListener("click", async () => {
  const titleBarStyle = windowSetTitleBarStyleTitleBarStyleSelect.value;
  windowSetTitleBarStyleInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setTitleBarStyle(titleBarStyle);
    windowSetTitleBarStyleInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetTitleBarStyleInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetTitleTitleInput = document.querySelector(
  "#window-set-title-title-input"
);
const windowSetTitleInvokeButton = document.querySelector(
  "#window-set-title-invoke-button"
);
const windowSetTitleInvokeResult = document.querySelector(
  "#window-set-title-invoke-result"
);
windowSetTitleInvokeButton.addEventListener("click", async () => {
  const title = windowSetTitleTitleInput.value;
  windowSetTitleInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setTitle(title);
    windowSetTitleInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetTitleInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowSetUrlUrlInput = document.querySelector(
  "#window-set-url-url-input"
);
const windowSetUrlInvokeButton = document.querySelector(
  "#window-set-url-invoke-button"
);
const windowSetUrlInvokeResult = document.querySelector(
  "#window-set-url-invoke-result"
);
windowSetUrlInvokeButton.addEventListener("click", async () => {
  const url = windowSetUrlUrlInput.value;
  windowSetUrlInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setUrl(url);
    windowSetUrlInvokeResult.textContent = `success`;
  } catch (error) {
    windowSetUrlInvokeResult.textContent = `error: ${error.message}`;
  }
});
const windowShowInvokeButton = document.querySelector(
  "#window-show-invoke-button"
);
const windowShowInvokeResult = document.querySelector(
  "#window-show-invoke-result"
);
windowShowInvokeButton.addEventListener("click", async () => {
  windowShowInvokeResult.textContent = "";
  try {
    await getSelectedWindow().show();
    windowShowInvokeResult.textContent = `success`;
  } catch (error) {
    windowShowInvokeResult.textContent = `error: ${error.message}`;
  }
});
