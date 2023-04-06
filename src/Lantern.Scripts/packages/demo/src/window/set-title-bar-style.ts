import { getSelectedWindow } from "./select-window";

const windowSetTitleBarStyleTitleBarStyleSelect =
  document.querySelector<HTMLSelectElement>(
    "#window-set-title-bar-style-title-bar-style-select"
  )!;
const windowSetTitleBarStyleInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-set-title-bar-style-invoke-button"
  )!;
const windowSetTitleBarStyleInvokeResult =
  document.querySelector<HTMLSpanElement>(
    "#window-set-title-bar-style-invoke-result"
  )!;

windowSetTitleBarStyleInvokeButton.addEventListener("click", async () => {
  const titleBarStyle = windowSetTitleBarStyleTitleBarStyleSelect.value;

  windowSetTitleBarStyleInvokeResult.textContent = "";
  try {
    await getSelectedWindow().setTitleBarStyle(titleBarStyle);
    windowSetTitleBarStyleInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowSetTitleBarStyleInvokeResult.textContent = `error: ${error.message}`;
  }
});
