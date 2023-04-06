import { getSelectedWindow } from "./select-window";

const windowGetMinSizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-get-min-size-invoke-button"
)!;
const windowGetMinSizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-get-min-size-invoke-result"
)!;

windowGetMinSizeInvokeButton.addEventListener("click", async () => {
  windowGetMinSizeInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getMinSize();
    windowGetMinSizeInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowGetMinSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
