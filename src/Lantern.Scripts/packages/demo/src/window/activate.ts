import { getSelectedWindow } from "./select-window";

const windowActivateInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-activate-invoke-button"
)!;
const windowActivateInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-activate-invoke-result"
)!;

windowActivateInvokeButton.addEventListener("click", async () => {
  windowActivateInvokeResult.textContent = "";
  try {
    await getSelectedWindow().activate();
    windowActivateInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowActivateInvokeResult.textContent = `error: ${error.message}`;
  }
});
