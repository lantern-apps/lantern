import { getSelectedWindow } from "./select-window";

const windowSetPositionXInput = document.querySelector<HTMLInputElement>(
  "#window-set-position-x-input"
)!;
const windowSetPositionYInput = document.querySelector<HTMLInputElement>(
  "#window-set-position-y-input"
)!;
const windowSetPositionInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-set-position-invoke-button"
)!;
const windowSetPositionInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-set-position-invoke-result"
)!;

windowSetPositionInvokeButton.addEventListener("click", async () => {
  const x = windowSetPositionXInput.valueAsNumber;
  const y = windowSetPositionYInput.valueAsNumber;

  windowSetPositionInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setPosition(x, y);
    windowSetPositionInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetPositionInvokeResult.textContent = `error: ${error.message}`;
  }
});
