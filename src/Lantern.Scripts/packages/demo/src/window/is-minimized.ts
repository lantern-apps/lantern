import { getSelectedWindow } from "./select-window";

const windowIsMinimizedInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-is-minimized-invoke-button"
)!;
const windowIsMinimizedInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-minimized-invoke-result"
)!;

windowIsMinimizedInvokeButton.addEventListener("click", async () => {
  windowIsMinimizedInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isMinimized();
    windowIsMinimizedInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsMinimizedInvokeResult.textContent = `error: ${error.message}`;
  }
});
