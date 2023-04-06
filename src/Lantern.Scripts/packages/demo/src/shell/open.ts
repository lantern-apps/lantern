import { open } from "@lantern-app/api/shell";

const shellOpenPathInput = document.querySelector<HTMLInputElement>(
  "#shell-open-path-input"
)!;
const shellOpenInvokeButton = document.querySelector<HTMLButtonElement>(
  "#shell-open-invoke-button"
)!;
const shellOpenInvokeResult = document.querySelector<HTMLSpanElement>(
  "#shell-open-invoke-result"
)!;

shellOpenInvokeButton.addEventListener("click", async () => {
  const path = shellOpenPathInput.value;

  shellOpenInvokeResult.textContent = "";
  try {
    await open(path);
    shellOpenInvokeResult.textContent = `success`;
  } catch (error: any) {
    shellOpenInvokeResult.textContent = `error: ${error.message}`;
  }
});
