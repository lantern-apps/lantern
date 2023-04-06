import { getSelectedWindow } from "./select-window";

const windowIsFullScreenInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-is-full-screen-invoke-button"
  )!;
const windowIsFullScreenInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-full-screen-invoke-result"
)!;

windowIsFullScreenInvokeButton.addEventListener("click", async () => {
  windowIsFullScreenInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isFullScreen();
    windowIsFullScreenInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsFullScreenInvokeResult.textContent = `error: ${error.message}`;
  }
});
