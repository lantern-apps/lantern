import { getSelectedWindow } from "./select-window";

const windowGetPositionInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-get-position-invoke-button"
)!;
const windowGetPositionInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-get-position-invoke-result"
)!;

windowGetPositionInvokeButton.addEventListener("click", async () => {
  windowGetPositionInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getPosition();
    windowGetPositionInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    windowGetPositionInvokeResult.textContent = `error: ${error.message}`;
  }
});
