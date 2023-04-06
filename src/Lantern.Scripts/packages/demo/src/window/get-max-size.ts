import { getSelectedWindow } from "./select-window";

const windowGetMaxSizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-get-max-size-invoke-button"
)!;
const windowGetMaxSizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-get-max-size-invoke-result"
)!;

windowGetMaxSizeInvokeButton.addEventListener("click", async () => {
  windowGetMaxSizeInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getMaxSize();
    windowGetMaxSizeInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowGetMaxSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
