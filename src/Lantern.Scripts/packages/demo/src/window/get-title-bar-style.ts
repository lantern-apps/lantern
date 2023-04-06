import { getSelectedWindow } from "./select-window";

const windowGetTitleBarStyleInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-get-title-bar-style-invoke-button"
  )!;
const windowGetTitleBarStyleInvokeResult =
  document.querySelector<HTMLSpanElement>(
    "#window-get-title-bar-style-invoke-result"
  )!;

windowGetTitleBarStyleInvokeButton.addEventListener("click", async () => {
  windowGetTitleBarStyleInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getTitleBarStyle();
    windowGetTitleBarStyleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowGetTitleBarStyleInvokeResult.textContent = `error: ${error.message}`;
  }
});
