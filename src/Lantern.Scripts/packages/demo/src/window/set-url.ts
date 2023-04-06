import { getSelectedWindow } from "./select-window";

const windowSetUrlUrlInput = document.querySelector<HTMLInputElement>(
  "#window-set-url-url-input"
)!;
const windowSetUrlInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-set-url-invoke-button"
)!;
const windowSetUrlInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-set-url-invoke-result"
)!;

windowSetUrlInvokeButton.addEventListener("click", async () => {
  const url = windowSetUrlUrlInput.value;

  windowSetUrlInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setUrl(url);
    windowSetUrlInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetUrlInvokeResult.textContent = `error: ${error.message}`;
  }
});
