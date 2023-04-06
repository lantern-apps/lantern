import { getSelectedWindow } from "./select-window";

const windowIsResizableInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-is-resizable-invoke-button"
)!;
const windowIsResizableInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-resizable-invoke-result"
)!;

windowIsResizableInvokeButton.addEventListener("click", async () => {
  windowIsResizableInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isResizable();
    windowIsResizableInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsResizableInvokeResult.textContent = `error: ${error.message}`;
  }
});
