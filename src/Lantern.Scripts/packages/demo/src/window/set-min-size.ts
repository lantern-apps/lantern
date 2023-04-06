import { getSelectedWindow } from "./select-window";

const windowSetMinSizeWidthInput = document.querySelector<HTMLInputElement>(
  "#window-set-min-size-width-input"
)!;
const windowSetMinSizeHeightInput = document.querySelector<HTMLInputElement>(
  "#window-set-min-size-height-input"
)!;
const windowSetMinSizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-set-min-size-invoke-button"
)!;
const windowSetMinSizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-set-min-size-invoke-result"
)!;

windowSetMinSizeInvokeButton.addEventListener("click", async () => {
  const width = windowSetMinSizeWidthInput.valueAsNumber;
  const height = windowSetMinSizeHeightInput.valueAsNumber;

  windowSetMinSizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setMinSize(width, height);
    windowSetMinSizeInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetMinSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
