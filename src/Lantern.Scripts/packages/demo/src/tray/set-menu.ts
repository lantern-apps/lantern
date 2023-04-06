import { setMenu } from "@lantern-app/api/tray";

const traySetMenuItemsTextarea = document.querySelector<HTMLTextAreaElement>(
  "#tray-set-menu-items-textarea"
)!;
const traySetMenuInvokeButton = document.querySelector<HTMLButtonElement>(
  "#tray-set-menu-invoke-button"
)!;
const traySetMenuInvokeResult = document.querySelector<HTMLSpanElement>(
  "#tray-set-menu-invoke-result"
)!;

traySetMenuInvokeButton.addEventListener("click", async () => {
  const items = eval(traySetMenuItemsTextarea.value);

  traySetMenuInvokeResult.textContent = "";
  try {
    await setMenu(items);
    traySetMenuInvokeResult.textContent = `success`;
  } catch (error: any) {
    traySetMenuInvokeResult.textContent = `error: ${error.message}`;
  }
});
