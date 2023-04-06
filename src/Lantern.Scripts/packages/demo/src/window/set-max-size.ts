import { getSelectedWindow } from "./select-window";

const windowSetMaxSizeWidthInput = document.querySelector<HTMLInputElement>(
  "#window-set-max-size-width-input"
)!;
const windowSetMaxSizeHeightInput = document.querySelector<HTMLInputElement>(
  "#window-set-max-size-height-input"
)!;
const windowSetMaxSizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-set-max-size-invoke-button"
)!;
const windowSetMaxSizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-set-max-size-invoke-result"
)!;

windowSetMaxSizeInvokeButton.addEventListener("click", async () => {
  const width = windowSetMaxSizeWidthInput.valueAsNumber;
  const height = windowSetMaxSizeHeightInput.valueAsNumber;

  windowSetMaxSizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setMaxSize(width, height);
    windowSetMaxSizeInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetMaxSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
