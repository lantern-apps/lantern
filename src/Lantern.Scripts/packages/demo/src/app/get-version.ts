import { getVersion } from "@lantern-app/api/app";

const appGetVersionInvokeButton = document.querySelector<HTMLButtonElement>(
  "#app-get-version-invoke-button"
)!;
const appGetVersionInvokeResult = document.querySelector<HTMLSpanElement>(
  "#app-get-version-invoke-result"
)!;

appGetVersionInvokeButton.addEventListener("click", async () => {
  appGetVersionInvokeResult.textContent = "";
  try {
    const result = await getVersion();
    appGetVersionInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    appGetVersionInvokeResult.textContent = `error: ${error.message}`;
  }
});
