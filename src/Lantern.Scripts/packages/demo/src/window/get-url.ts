import { getSelectedWindow } from "./select-window";

const windowGetUrlInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-get-url-invoke-button"
)!;
const windowGetUrlInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-get-url-invoke-result"
)!;

windowGetUrlInvokeButton.addEventListener("click", async () => {
  windowGetUrlInvokeResult.textContent = "";
  try {
    const result = await getSelectedWindow().getUrl();
    windowGetUrlInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error: any) {
    windowGetUrlInvokeResult.textContent = `error: ${error.message}`;
  }
});
