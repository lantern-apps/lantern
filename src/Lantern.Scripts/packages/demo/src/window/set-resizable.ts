import { getSelectedWindow } from "./select-window";

const windowSetResizableResizableInput =
  document.querySelector<HTMLInputElement>(
    "#window-set-resizable-resizable-input"
  )!;
const windowSetResizableInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-set-resizable-invoke-button"
  )!;
const windowSetResizableInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-set-resizable-invoke-result"
)!;

windowSetResizableInvokeButton.addEventListener("click", async () => {
  const resizable = windowSetResizableResizableInput.checked;

  windowSetResizableInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setResizable(resizable);
    windowSetResizableInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetResizableInvokeResult.textContent = `error: ${error.message}`;
  }
});
