import { check } from "@lantern-app/api/updater";

const updaterCheckInvokeButton = document.querySelector<HTMLButtonElement>(
  "#updater-check-invoke-button"
)!;
const updaterCheckInvokeResult = document.querySelector<HTMLSpanElement>(
  "#updater-check-invoke-result"
)!;

updaterCheckInvokeButton.addEventListener("click", async () => {
  updaterCheckInvokeResult.textContent = "";
  try {
    const result = await check();
    updaterCheckInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error: any) {
    updaterCheckInvokeResult.textContent = `error: ${error.message}`;
  }
});
