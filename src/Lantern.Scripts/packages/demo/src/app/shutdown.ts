import { shutdown } from "@lantern-app/api/app";

const appShutdownInvokeButton = document.querySelector<HTMLButtonElement>(
  "#app-shutdown-invoke-button"
)!;
const appShutdownInvokeResult = document.querySelector<HTMLSpanElement>(
  "#app-shutdown-invoke-result"
)!;

appShutdownInvokeButton.addEventListener("click", async () => {
  appShutdownInvokeResult.textContent = "";
  try {
    await shutdown();
    appShutdownInvokeResult.textContent = `success`;
  } catch (error: any) {
    appShutdownInvokeResult.textContent = `error: ${error.message}`;
  }
});
