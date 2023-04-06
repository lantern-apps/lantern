import { hide } from "@lantern-app/api/tray";

const trayHideInvokeButton = document.querySelector<HTMLButtonElement>(
  "#tray-hide-invoke-button"
)!;
const trayHideInvokeResult = document.querySelector<HTMLSpanElement>(
  "#tray-hide-invoke-result"
)!;

trayHideInvokeButton.addEventListener("click", async () => {
  trayHideInvokeResult.textContent = "";
  try {
    await hide();
    trayHideInvokeResult.textContent = `success`;
  } catch (error: any) {
    trayHideInvokeResult.textContent = `error: ${error.message}`;
  }
});
