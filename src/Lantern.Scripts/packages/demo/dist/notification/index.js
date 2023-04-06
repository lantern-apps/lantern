import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
function send(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.notification.send", options);
}
const notificationSendTitleInput = document.querySelector(
  "#notification-send-title-input"
);
const notificationSendBodyInput = document.querySelector(
  "#notification-send-body-input"
);
const notificationSendInvokeButton = document.querySelector(
  "#notification-send-invoke-button"
);
const notificationSendInvokeResult = document.querySelector(
  "#notification-send-invoke-result"
);
notificationSendInvokeButton.addEventListener("click", async () => {
  const title = notificationSendTitleInput.value;
  const body = notificationSendBodyInput.value;
  notificationSendInvokeResult.textContent = "";
  try {
    await send({
      title,
      body
    });
    notificationSendInvokeResult.textContent = `success`;
  } catch (error) {
    notificationSendInvokeResult.textContent = `error: ${error.message}`;
  }
});
