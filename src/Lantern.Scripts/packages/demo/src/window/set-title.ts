import { getSelectedWindow } from "./select-window";

const windowSetTitleTitleInput = document.querySelector<HTMLInputElement>(
  "#window-set-title-title-input"
)!;
const windowSetTitleInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-set-title-invoke-button"
)!;
const windowSetTitleInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-set-title-invoke-result"
)!;

windowSetTitleInvokeButton.addEventListener("click", async () => {
  const title = windowSetTitleTitleInput.value;

  windowSetTitleInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setTitle(title);
    windowSetTitleInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetTitleInvokeResult.textContent = `error: ${error.message}`;
  }
});
