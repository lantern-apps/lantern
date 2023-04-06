import { getSelectedWindow } from "./select-window";

const windowIsSkipTaskbarInvokeButton =
  document.querySelector<HTMLButtonElement>(
    "#window-is-skip-taskbar-invoke-button"
  )!;
const windowIsSkipTaskbarInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-is-skip-taskbar-invoke-result"
)!;

windowIsSkipTaskbarInvokeButton.addEventListener("click", async () => {
  windowIsSkipTaskbarInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().isSkipTaskbar();
    windowIsSkipTaskbarInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowIsSkipTaskbarInvokeResult.textContent = `error: ${error.message}`;
  }
});
