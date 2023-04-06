import { getSelectedWindow } from "./select-window";

const windowIsMaximizedInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-is-maximized-invoke-button"
)!;
const windowIsMaximizedInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-maximized-invoke-result"
)!;

windowIsMaximizedInvokeButton.addEventListener("click", async () => {
  windowIsMaximizedInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isMaximized();
    windowIsMaximizedInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsMaximizedInvokeResult.textContent = `error: ${error.message}`;
  }
});
