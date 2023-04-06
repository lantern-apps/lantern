import { getSelectedWindow } from "./select-window";

const windowHideInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-hide-invoke-button"
)!;
const windowHideInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-hide-invoke-result"
)!;

windowHideInvokeButton.addEventListener("click", async () => {
  windowHideInvokeResult.textContent = "";
  try {
    await getSelectedWindow().hide();
    windowHideInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowHideInvokeResult.textContent = `error: ${error.message}`;
  }
});
