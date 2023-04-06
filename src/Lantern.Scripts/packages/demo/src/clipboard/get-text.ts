import { getText } from "@lantern-app/api/clipboard";

const clipboardGetTextInvokeButton = document.querySelector<HTMLButtonElement>(
  "#clipboard-get-text-invoke-button"
)!;
const clipboardGetTextInvokeResult = document.querySelector<HTMLSpanElement>(
  "#clipboard-get-text-invoke-result"
)!;

clipboardGetTextInvokeButton.addEventListener("click", async () => {
  clipboardGetTextInvokeResult.textContent = "";
  try {
    const result = await getText();
    clipboardGetTextInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    clipboardGetTextInvokeResult.textContent = `error: ${error.message}`;
  }
});
