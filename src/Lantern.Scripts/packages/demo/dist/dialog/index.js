import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
function message(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.message", options);
}
function ask(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.ask", options);
}
function confirm(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.confirm", options);
}
function openFile(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.openFile", options);
}
function openFolder(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.openFolder", options);
}
function saveFile(options) {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.dialog.saveFile", options);
}
const dialogAskTitleInput = document.querySelector(
  "#dialog-ask-title-input"
);
const dialogAskBodyInput = document.querySelector(
  "#dialog-ask-body-input"
);
const dialogAskInvokeButton = document.querySelector(
  "#dialog-ask-invoke-button"
);
const dialogAskInvokeResult = document.querySelector(
  "#dialog-ask-invoke-result"
);
dialogAskInvokeButton.addEventListener("click", async () => {
  const title2 = dialogAskTitleInput.value;
  const body = dialogAskBodyInput.value;
  dialogAskInvokeResult.textContent = "";
  try {
    const result = await ask({
      title: title2,
      body
    });
    dialogAskInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error) {
    dialogAskInvokeResult.textContent = `error: ${error.ask}`;
  }
});
const dialogConfirmTitleInput = document.querySelector(
  "#dialog-confirm-title-input"
);
const dialogConfirmBodyInput = document.querySelector(
  "#dialog-confirm-body-input"
);
const dialogConfirmInvokeButton = document.querySelector(
  "#dialog-confirm-invoke-button"
);
const dialogConfirmInvokeResult = document.querySelector(
  "#dialog-confirm-invoke-result"
);
dialogConfirmInvokeButton.addEventListener("click", async () => {
  const title2 = dialogConfirmTitleInput.value;
  const body = dialogConfirmBodyInput.value;
  dialogConfirmInvokeResult.textContent = "";
  try {
    const result = await confirm({
      title: title2,
      body
    });
    dialogConfirmInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    dialogConfirmInvokeResult.textContent = `error: ${error.confirm}`;
  }
});
const dialogMessageTitleInput = document.querySelector(
  "#dialog-message-title-input"
);
const dialogMessageBodyInput = document.querySelector(
  "#dialog-message-body-input"
);
const dialogMessageInvokeButton = document.querySelector(
  "#dialog-message-invoke-button"
);
const dialogMessageInvokeResult = document.querySelector(
  "#dialog-message-invoke-result"
);
dialogMessageInvokeButton.addEventListener("click", async () => {
  const title2 = dialogMessageTitleInput.value;
  const body = dialogMessageBodyInput.value;
  dialogMessageInvokeResult.textContent = "";
  try {
    await message({
      title: title2,
      body
    });
    dialogMessageInvokeResult.textContent = `success`;
  } catch (error) {
    dialogMessageInvokeResult.textContent = `error: ${error.message}`;
  }
});
const dialogOpenFileTitleInput = document.querySelector(
  "#dialog-open-file-title-input"
);
const dialogOpenFileDefaultPathInput = document.querySelector(
  "#dialog-open-file-default-path-input"
);
const dialogOpenFileFiltersTextarea = document.querySelector(
  "#dialog-open-file-filters-textarea"
);
const dialogOpenFileMultipleInput = document.querySelector(
  "#dialog-open-file-multiple-input"
);
const dialogOpenFileInvokeButton = document.querySelector(
  "#dialog-open-file-invoke-button"
);
const dialogOpenFileInvokeResult = document.querySelector(
  "#dialog-open-file-invoke-result"
);
dialogOpenFileInvokeButton.addEventListener("click", async () => {
  const title = dialogOpenFileTitleInput.value;
  const defaultPath = dialogOpenFileDefaultPathInput.value;
  const filters = eval(dialogOpenFileFiltersTextarea.value);
  const multiple = dialogOpenFileMultipleInput.checked;
  dialogOpenFileInvokeResult.textContent = "";
  try {
    const result = await openFile({
      title,
      defaultPath,
      filters,
      multiple
    });
    dialogOpenFileInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    dialogOpenFileInvokeResult.textContent = `error: ${error.message}`;
  }
});
const dialogOpenFolderTitleInput = document.querySelector(
  "#dialog-open-folder-title-input"
);
const dialogOpenFolderDefaultPathInput = document.querySelector(
  "#dialog-open-folder-default-path-input"
);
const dialogOpenFolderMultipleInput = document.querySelector(
  "#dialog-open-folder-multiple-input"
);
const dialogOpenFolderInvokeButton = document.querySelector(
  "#dialog-open-folder-invoke-button"
);
const dialogOpenFolderInvokeResult = document.querySelector(
  "#dialog-open-folder-invoke-result"
);
dialogOpenFolderInvokeButton.addEventListener("click", async () => {
  const title2 = dialogOpenFolderTitleInput.value;
  const defaultPath2 = dialogOpenFolderDefaultPathInput.value;
  const multiple2 = dialogOpenFolderMultipleInput.checked;
  dialogOpenFolderInvokeResult.textContent = "";
  try {
    const result = await openFolder({
      title: title2,
      defaultPath: defaultPath2,
      multiple: multiple2
    });
    dialogOpenFolderInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    dialogOpenFolderInvokeResult.textContent = `error: ${error.message}`;
  }
});
const dialogSaveFileTitleInput = document.querySelector(
  "#dialog-save-file-title-input"
);
const dialogSaveFileDefaultPathInput = document.querySelector(
  "#dialog-save-file-default-path-input"
);
const dialogSaveFileFiltersTextarea = document.querySelector(
  "#dialog-save-file-filters-textarea"
);
const dialogSaveFileInvokeButton = document.querySelector(
  "#dialog-save-file-invoke-button"
);
const dialogSaveFileInvokeResult = document.querySelector(
  "#dialog-save-file-invoke-result"
);
dialogSaveFileInvokeButton.addEventListener("click", async () => {
  const title = dialogSaveFileTitleInput.value;
  const defaultPath = dialogSaveFileDefaultPathInput.value;
  const filters = eval(dialogSaveFileFiltersTextarea.value);
  dialogSaveFileInvokeResult.textContent = "";
  try {
    const result = await saveFile({
      title,
      defaultPath,
      filters
    });
    dialogSaveFileInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    dialogSaveFileInvokeResult.textContent = `error: ${error.message}`;
  }
});
