import { getSelectedWindow } from "./select-window";

const windowMaximizeInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-maximize-invoke-button"
)!;
const windowMaximizeInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-maximize-invoke-result"
)!;

windowMaximizeInvokeButton.addEventListener("click", async () => {
  windowMaximizeInvokeResult.textContent = "";
  try {
    await getSelectedWindow().maximize();
    windowMaximizeInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowMaximizeInvokeResult.textContent = `error: ${error.message}`;
  }
});
