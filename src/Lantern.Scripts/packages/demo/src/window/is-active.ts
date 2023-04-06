import { getSelectedWindow } from "./select-window";

const windowIsActiveInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-is-active-invoke-button"
)!;
const windowIsActiveInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-active-invoke-result"
)!;

windowIsActiveInvokeButton.addEventListener("click", async () => {
  windowIsActiveInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isActive();
    windowIsActiveInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsActiveInvokeResult.textContent = `error: ${error.message}`;
  }
});
