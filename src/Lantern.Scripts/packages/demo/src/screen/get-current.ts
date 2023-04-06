import { getCurrent } from "@lantern-app/api/screen";

const screenGetCurrentInvokeButton = document.querySelector<HTMLButtonElement>(
  "#screen-get-current-invoke-button"
)!;
const screenGetCurrentInvokeResult = document.querySelector<HTMLSpanElement>(
  "#screen-get-current-invoke-result"
)!;

screenGetCurrentInvokeButton.addEventListener("click", async () => {
  screenGetCurrentInvokeResult.textContent = "";
  try {
    const result = await getCurrent();
    screenGetCurrentInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    screenGetCurrentInvokeResult.textContent = `error: ${error.message}`;
  }
});
