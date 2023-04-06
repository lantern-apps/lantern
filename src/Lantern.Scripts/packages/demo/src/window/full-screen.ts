import { getSelectedWindow } from "./select-window";

const windowFullScreenInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-full-screen-invoke-button"
)!;
const windowFullScreenInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-full-screen-invoke-result"
)!;

windowFullScreenInvokeButton.addEventListener("click", async () => {
  windowFullScreenInvokeResult.textContent = "";
  try {
    await getSelectedWindow().fullScreen();
    windowFullScreenInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowFullScreenInvokeResult.textContent = `error: ${error.message}`;
  }
});
