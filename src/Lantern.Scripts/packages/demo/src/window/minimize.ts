import { getSelectedWindow } from "./select-window";

const windowMinimizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-minimize-invoke-button"
)!;
const windowMinimizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-minimize-invoke-result"
)!;

windowMinimizeInvokeButton.addEventListener("click", async () => {
  windowMinimizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().minimize();
    windowMinimizeInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowMinimizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
