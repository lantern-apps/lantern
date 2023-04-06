import { launch } from "@lantern-app/api/updater";

const updaterLaunchInvokeButton = document.querySelector<HTMLButtonElement>(
  "#updater-launch-invoke-button"
)!;
const updaterLaunchInvokeResult = document.querySelector<HTMLSpanElement>(
  "#updater-launch-invoke-result"
)!;

updaterLaunchInvokeButton.addEventListener("click", async () => {
  updaterLaunchInvokeResult.textContent = "";
  try {
    await launch();
    updaterLaunchInvokeResult.textContent = `success`;
  } catch (error: any) {
    updaterLaunchInvokeResult.textContent = `error: ${error.message}`;
  }
});
