import { perform } from "@lantern-app/api/updater";

const updaterPerformInvokeButton = document.querySelector<HTMLButtonElement>(
  "#updater-perform-invoke-button"
)!;
const updaterPerformInvokeResult = document.querySelector<HTMLSpanElement>(
  "#updater-perform-invoke-result"
)!;

updaterPerformInvokeButton.addEventListener("click", async () => {
  updaterPerformInvokeResult.textContent = "";
  try {
    await perform();
    updaterPerformInvokeResult.textContent = `success`;
  } catch (error: any) {
    updaterPerformInvokeResult.textContent = `error: ${error.message}`;
  }
});
