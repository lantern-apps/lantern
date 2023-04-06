import { getSelectedWindow } from "./select-window";

const windowSetSizeWidthInput = document.querySelector<HTMLInputElement>(
  "#window-set-size-width-input"
)!;
const windowSetSizeHeightInput = document.querySelector<HTMLInputElement>(
  "#window-set-size-height-input"
)!;
const windowSetSizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-set-size-invoke-button"
)!;
const windowSetSizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-set-size-invoke-result"
)!;

windowSetSizeInvokeButton.addEventListener("click", async () => {
  const width = windowSetSizeWidthInput.valueAsNumber;
  const height = windowSetSizeHeightInput.valueAsNumber;

  windowSetSizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setSize(width, height);
    windowSetSizeInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetSizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
