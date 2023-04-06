import { getSelectedWindow } from "./select-window";

const windowGetTitleInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-get-title-invoke-button"
)!;
const windowGetTitleInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-get-title-invoke-result"
)!;

windowGetTitleInvokeButton.addEventListener("click", async () => {
  windowGetTitleInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getTitle();
    windowGetTitleInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowGetTitleInvokeResult.textContent = `error: ${error.message}`;
  }
});
