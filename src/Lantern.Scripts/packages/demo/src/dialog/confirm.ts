import { confirm } from "@lantern-app/api/dialog";

const dialogConfirmTitleInput = document.querySelector<HTMLInputElement>(
  "#dialog-confirm-title-input"
)!;
const dialogConfirmBodyInput = document.querySelector<HTMLInputElement>(
  "#dialog-confirm-body-input"
)!;
const dialogConfirmInvokeButton = document.querySelector<HTMLButtonElement>(
  "#dialog-confirm-invoke-button"
)!;
const dialogConfirmInvokeResult = document.querySelector<HTMLSpanElement>(
  "#dialog-confirm-invoke-result"
)!;

dialogConfirmInvokeButton.addEventListener("click", async () => {
  const title = dialogConfirmTitleInput.value;
  const body = dialogConfirmBodyInput.value;

  dialogConfirmInvokeResult.textContent = "";
  try {
    const result = await confirm({
      title,
      body,
    });
    dialogConfirmInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    dialogConfirmInvokeResult.textContent = `error: ${error.confirm}`;
  }
});
