import { setText } from "@lantern-app/api/clipboard";

const clipboardSetTextTextInput = document.querySelector<HTMLInputElement>(
  "#clipboard-set-text-text-input"
)!;
const clipboardSetTextInvokeButton = document.querySelector<HTMLButtonElement>(
  "#clipboard-set-text-invoke-button"
)!;
const clipboardSetTextInvokeResult = document.querySelector<HTMLSpanElement>(
  "#clipboard-set-text-invoke-result"
)!;

clipboardSetTextInvokeButton.addEventListener("click", async () => {
  const text = clipboardSetTextTextInput.value;

  clipboardSetTextInvokeResult.textContent = "";
  try {
    await setText(text);
    clipboardSetTextInvokeResult.textContent = `success`;
  } catch (error: any) {
    clipboardSetTextInvokeResult.textContent = `error: ${error.message}`;
  }
});
