import { getSelectedWindow } from "./select-window";

const windowSetSkipTaskbarSkipTaskbarInput =
  document.querySelector<HTMLInputElement>(
    "#window-set-skip-taskbar-skip-taskbar-input"
  )!;
const windowSetSkipTaskbarInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-set-skip-taskbar-invoke-button"
  )!;
const windowSetSkipTaskbarInvokeResult =
  document.querySelector<HTMLSpanElement>(
    "#window-set-skip-taskbar-invoke-result"
  )!;

windowSetSkipTaskbarInvokeButton.addEventListener("click", async () => {
  const skipTaskbar = windowSetSkipTaskbarSkipTaskbarInput.checked;

  windowSetSkipTaskbarInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setSkipTaskbar(skipTaskbar);
    windowSetSkipTaskbarInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetSkipTaskbarInvokeResult.textContent = `error: ${error.message}`;
  }
});
