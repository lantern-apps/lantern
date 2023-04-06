import { getSelectedWindow } from "./select-window";

const windowCenterInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-center-invoke-button"
)!;
const windowCenterInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-center-invoke-result"
)!;

windowCenterInvokeButton.addEventListener("click", async () => {
  windowCenterInvokeResult.textContent = "";
  try {
    await getSelectedWindow().center();
    windowCenterInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowCenterInvokeResult.textContent = `error: ${error.message}`;
  }
});
