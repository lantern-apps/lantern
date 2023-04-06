import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
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
function setMenu(items2) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.tray.setMenu", {
    items: items2
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
const trayHideInvokeButton = document.querySelector(
  "#tray-hide-invoke-button"
);
const trayHideInvokeResult = document.querySelector(
  "#tray-hide-invoke-result"
);
trayHideInvokeButton.addEventListener("click", async () => {
  trayHideInvokeResult.textContent = "";
  try {
    await hide();
    trayHideInvokeResult.textContent = `success`;
  } catch (error) {
    trayHideInvokeResult.textContent = `error: ${error.message}`;
  }
});
const trayIsVisibleInvokeButton = document.querySelector(
  "#tray-is-visible-invoke-button"
);
const trayIsVisibleInvokeResult = document.querySelector(
  "#tray-is-visible-invoke-result"
);
trayIsVisibleInvokeButton.addEventListener("click", async () => {
  trayIsVisibleInvokeResult.textContent = "";
  try {
    const result = await isVisible();
    trayIsVisibleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    trayIsVisibleInvokeResult.textContent = `error: ${error.message}`;
  }
});
const trayOnClickListenButton = document.querySelector(
  "#tray-on-click-listen-button"
);
const trayOnClickUnlistenButton = document.querySelector(
  "#tray-on-click-unlisten-button"
);
const trayOnClickListenResult = document.querySelector(
  "#tray-on-click-listen-result"
);
let subscription$1;
trayOnClickListenButton.addEventListener("click", async () => {
  if (subscription$1) {
    subscription$1.unsubscribe();
  }
  let times = 0;
  trayOnClickListenResult.textContent = "";
  try {
    subscription$1 = onClick().subscribe((event) => {
      trayOnClickListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error) {
    trayOnClickListenResult.textContent = `error: ${error.message}`;
  }
});
trayOnClickUnlistenButton.addEventListener("click", async () => {
  try {
    offClick();
  } catch (error) {
    trayOnClickListenResult.textContent = `error: ${error.message}`;
  }
});
const trayOnMenuItemClickListenButton = document.querySelector(
  "#tray-on-menu-item-click-listen-button"
);
const trayOnMenuItemClickUnlistenButton = document.querySelector(
  "#tray-on-menu-item-click-unlisten-button"
);
const trayOnMenuItemClickListenResult = document.querySelector(
  "#tray-on-menu-item-click-listen-result"
);
let subscription;
trayOnMenuItemClickListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }
  let times = 0;
  trayOnMenuItemClickListenResult.textContent = "";
  try {
    subscription = onMenuItemClick().subscribe((event) => {
      trayOnMenuItemClickListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error) {
    trayOnMenuItemClickListenResult.textContent = `error: ${error.message}`;
  }
});
trayOnMenuItemClickUnlistenButton.addEventListener("click", async () => {
  try {
    offMenuItemClick();
  } catch (error) {
    trayOnMenuItemClickListenResult.textContent = `error: ${error.message}`;
  }
});
const traySetMenuItemsTextarea = document.querySelector(
  "#tray-set-menu-items-textarea"
);
const traySetMenuInvokeButton = document.querySelector(
  "#tray-set-menu-invoke-button"
);
const traySetMenuInvokeResult = document.querySelector(
  "#tray-set-menu-invoke-result"
);
traySetMenuInvokeButton.addEventListener("click", async () => {
  const items = eval(traySetMenuItemsTextarea.value);
  traySetMenuInvokeResult.textContent = "";
  try {
    await setMenu(items);
    traySetMenuInvokeResult.textContent = `success`;
  } catch (error) {
    traySetMenuInvokeResult.textContent = `error: ${error.message}`;
  }
});
const traySetTooltipTooltipInput = document.querySelector(
  "#tray-set-tooltip-tooltip-input"
);
const traySetTooltipInvokeButton = document.querySelector(
  "#tray-set-tooltip-invoke-button"
);
const traySetTooltipInvokeResult = document.querySelector(
  "#tray-set-tooltip-invoke-result"
);
traySetTooltipInvokeButton.addEventListener("click", async () => {
  const tooltip = traySetTooltipTooltipInput.value;
  traySetTooltipInvokeResult.textContent = "";
  try {
    await setTooltip(tooltip);
    traySetTooltipInvokeResult.textContent = `success`;
  } catch (error) {
    traySetTooltipInvokeResult.textContent = `error: ${error.message}`;
  }
});
const trayShowInvokeButton = document.querySelector(
  "#tray-show-invoke-button"
);
const trayShowInvokeResult = document.querySelector(
  "#tray-show-invoke-result"
);
trayShowInvokeButton.addEventListener("click", async () => {
  trayShowInvokeResult.textContent = "";
  try {
    await show();
    trayShowInvokeResult.textContent = `success`;
  } catch (error) {
    trayShowInvokeResult.textContent = `error: ${error.message}`;
  }
});
