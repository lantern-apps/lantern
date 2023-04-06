import { getSelectedWindow } from "./select-window";

const windowIsAlwaysOnTopInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-is-always-on-top-invoke-button"
  )!;
const windowIsAlwaysOnTopInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-always-on-top-invoke-result"
)!;

windowIsAlwaysOnTopInvokeButton.addEventListener("click", async () => {
  windowIsAlwaysOnTopInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isAlwaysOnTop();
    windowIsAlwaysOnTopInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsAlwaysOnTopInvokeResult.textContent = `error: ${error.message}`;
  }
});
