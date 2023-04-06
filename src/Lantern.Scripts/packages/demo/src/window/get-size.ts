import { getSelectedWindow } from "./select-window";

const windowGetSizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-get-size-invoke-button"
)!;
const windowGetSizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-get-size-invoke-result"
)!;

windowGetSizeInvokeButton.addEventListener("click", async () => {
  windowGetSizeInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getSize();
    windowGetSizeInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowGetSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
