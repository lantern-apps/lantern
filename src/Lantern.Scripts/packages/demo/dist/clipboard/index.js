import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
function setText(text) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.clipboard.setText", text);
}
function getText() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.clipboard.getText");
}
const clipboardSetTextTextInput = document.querySelector(
  "#clipboard-set-text-text-input"
);
const clipboardSetTextInvokeButton = document.querySelector(
  "#clipboard-set-text-invoke-button"
);
const clipboardSetTextInvokeResult = document.querySelector(
  "#clipboard-set-text-invoke-result"
);
clipboardSetTextInvokeButton.addEventListener("click", async () => {
  const text = clipboardSetTextTextInput.value;
  clipboardSetTextInvokeResult.textContent = "";
  try {
    await setText(text);
    clipboardSetTextInvokeResult.textContent = `success`;
  } catch (error) {
    clipboardSetTextInvokeResult.textContent = `error: ${error.message}`;
  }
});
const clipboardGetTextInvokeButton = document.querySelector(
  "#clipboard-get-text-invoke-button"
);
const clipboardGetTextInvokeResult = document.querySelector(
  "#clipboard-get-text-invoke-result"
);
clipboardGetTextInvokeButton.addEventListener("click", async () => {
  clipboardGetTextInvokeResult.textContent = "";
  try {
    const result = await getText();
    clipboardGetTextInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    clipboardGetTextInvokeResult.textContent = `error: ${error.message}`;
  }
});
