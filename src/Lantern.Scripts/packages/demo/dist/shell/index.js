import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
function open(path) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.shell.open", {
    path
  });
}
const shellOpenPathInput = document.querySelector(
  "#shell-open-path-input"
);
const shellOpenInvokeButton = document.querySelector(
  "#shell-open-invoke-button"
);
const shellOpenInvokeResult = document.querySelector(
  "#shell-open-invoke-result"
);
shellOpenInvokeButton.addEventListener("click", async () => {
  const path = shellOpenPathInput.value;
  shellOpenInvokeResult.textContent = "";
  try {
    await open(path);
    shellOpenInvokeResult.textContent = `success`;
  } catch (error) {
    shellOpenInvokeResult.textContent = `error: ${error.message}`;
  }
});
