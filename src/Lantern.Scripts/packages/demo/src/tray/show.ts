import { show } from "@lantern-app/api/tray";

const trayShowInvokeButton = document.querySelector<HTMLButtonElement>(
  "#tray-show-invoke-button"
)!;
const trayShowInvokeResult = document.querySelector<HTMLSpanElement>(
  "#tray-show-invoke-result"
)!;

trayShowInvokeButton.addEventListener("click", async () => {
  trayShowInvokeResult.textContent = "";
  try {
    await show();
    trayShowInvokeResult.textContent = `success`;
  } catch (error: any) {
    trayShowInvokeResult.textContent = `error: ${error.message}`;
  }
});
