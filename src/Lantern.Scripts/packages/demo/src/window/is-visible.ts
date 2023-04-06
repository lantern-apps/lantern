import { getSelectedWindow } from "./select-window";

const windowIsVisibleInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-is-visible-invoke-button"
)!;
const windowIsVisibleInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-visible-invoke-result"
)!;

windowIsVisibleInvokeButton.addEventListener("click", async () => {
  windowIsVisibleInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isVisible();
    windowIsVisibleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsVisibleInvokeResult.textContent = `error: ${error.message}`;
  }
});
