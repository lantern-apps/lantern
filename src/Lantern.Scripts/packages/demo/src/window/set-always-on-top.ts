import { getSelectedWindow } from "./select-window";

const windowSetAlwaysOnTopAlwaysOnTopInput =
  document.querySelector<HTMLInputElement>(
    "#window-set-always-on-top-always-on-top-input"
  )!;
const windowSetAlwaysOnTopInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-set-always-on-top-invoke-button"
  )!;
const windowSetAlwaysOnTopInvokeResult =
  document.querySelector<HTMLSpanElement>(
    "#window-set-always-on-top-invoke-result"
  )!;

windowSetAlwaysOnTopInvokeButton.addEventListener("click", async () => {
  const alwaysOnTop = windowSetAlwaysOnTopAlwaysOnTopInput.checked;

  windowSetAlwaysOnTopInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setAlwaysOnTop(alwaysOnTop);
    windowSetAlwaysOnTopInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetAlwaysOnTopInvokeResult.textContent = `error: ${error.message}`;
  }
});
