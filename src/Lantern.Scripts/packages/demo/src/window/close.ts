import { getSelectedWindow } from "./select-window";

const windowCloseInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-close-invoke-button"
)!;
const windowCloseInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-close-invoke-result"
)!;

windowCloseInvokeButton.addEventListener("click", async () => {
  windowCloseInvokeResult.textContent = "";
  try {
    await getSelectedWindow().close();
    windowCloseInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowCloseInvokeResult.textContent = `error: ${error.message}`;
  }
});
