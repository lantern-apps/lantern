import { getSelectedWindow } from "./select-window";

const windowRestoreInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-restore-invoke-button"
)!;
const windowRestoreInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-restore-invoke-result"
)!;

windowRestoreInvokeButton.addEventListener("click", async () => {
  windowRestoreInvokeResult.textContent = "";
  try {
    await getSelectedWindow().restore();
    windowRestoreInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowRestoreInvokeResult.textContent = `error: ${error.message}`;
  }
});
