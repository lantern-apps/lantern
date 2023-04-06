import { send } from "@lantern-app/api/notification";

const notificationSendTitleInput = document.querySelector<HTMLInputElement>(
  "#notification-send-title-input"
)!;
const notificationSendBodyInput = document.querySelector<HTMLInputElement>(
  "#notification-send-body-input"
)!;
const notificationSendInvokeButton = document.querySelector<HTMLButtonElement>(
  "#notification-send-invoke-button"
)!;
const notificationSendInvokeResult = document.querySelector<HTMLSpanElement>(
  "#notification-send-invoke-result"
)!;

notificationSendInvokeButton.addEventListener("click", async () => {
  const title = notificationSendTitleInput.value;
  const body = notificationSendBodyInput.value;

  notificationSendInvokeResult.textContent = "";
  try {
    await send({
      title,
      body,
    });
    notificationSendInvokeResult.textContent = `success`;
  } catch (error: any) {
    notificationSendInvokeResult.textContent = `error: ${error.message}`;
  }
});
