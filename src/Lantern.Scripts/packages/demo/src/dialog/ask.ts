import { ask } from "@lantern-app/api/dialog";

const dialogAskTitleInput = document.querySelector<HTMLInputElement>(
  "#dialog-ask-title-input"
)!;
const dialogAskBodyInput = document.querySelector<HTMLInputElement>(
  "#dialog-ask-body-input"
)!;
const dialogAskInvokeButton = document.querySelector<HTMLButtonElement>(
  "#dialog-ask-invoke-button"
)!;
const dialogAskInvokeResult = document.querySelector<HTMLSpanElement>(
  "#dialog-ask-invoke-result"
)!;

dialogAskInvokeButton.addEventListener("click", async () => {
  const title = dialogAskTitleInput.value;
  const body = dialogAskBodyInput.value;

  dialogAskInvokeResult.textContent = "";
  try {
    const result = await ask({
      title,
      body,
    });
    dialogAskInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error: any) {
    dialogAskInvokeResult.textContent = `error: ${error.ask}`;
  }
});
