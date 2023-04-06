import { getSelectedWindow } from "./select-window";

const windowShowInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-show-invoke-button"
)!;
const windowShowInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-show-invoke-result"
)!;

windowShowInvokeButton.addEventListener("click", async () => {
  windowShowInvokeResult.textContent = "";
  try {
    await getSelectedWindow().show();
    windowShowInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowShowInvokeResult.textContent = `error: ${error.message}`;
  }
});
