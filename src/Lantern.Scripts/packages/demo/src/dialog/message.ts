import { message } from "@lantern-app/api/dialog";

const dialogMessageTitleInput = document.querySelector<HTMLInputElement>(
  "#dialog-message-title-input"
)!;
const dialogMessageBodyInput = document.querySelector<HTMLInputElement>(
  "#dialog-message-body-input"
)!;
const dialogMessageInvokeButton = document.querySelector<HTMLButtonElement>(
  "#dialog-message-invoke-button"
)!;
const dialogMessageInvokeResult = document.querySelector<HTMLSpanElement>(
  "#dialog-message-invoke-result"
)!;

dialogMessageInvokeButton.addEventListener("click", async () => {
  const title = dialogMessageTitleInput.value;
  const body = dialogMessageBodyInput.value;

  dialogMessageInvokeResult.textContent = "";
  try {
    await message({
      title,
      body,
    });
    dialogMessageInvokeResult.textContent = `success`;
  } catch (error: any) {
    dialogMessageInvokeResult.textContent = `error: ${error.message}`;
  }
});
