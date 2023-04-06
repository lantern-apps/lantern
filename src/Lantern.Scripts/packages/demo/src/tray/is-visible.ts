import { isVisible } from "@lantern-app/api/tray";

const trayIsVisibleInvokeButton = document.querySelector<HTMLButtonElement>(
  "#tray-is-visible-invoke-button"
)!;
const trayIsVisibleInvokeResult = document.querySelector<HTMLSpanElement>(
  "#tray-is-visible-invoke-result"
)!;

trayIsVisibleInvokeButton.addEventListener("click", async () => {
  trayIsVisibleInvokeResult.textContent = "";
  try {
    const result = await isVisible();
    trayIsVisibleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    trayIsVisibleInvokeResult.textContent = `error: ${error.message}`;
  }
});
