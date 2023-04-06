import { saveFile } from "@lantern-app/api/dialog";

const dialogSaveFileTitleInput = document.querySelector<HTMLInputElement>(
  "#dialog-save-file-title-input"
)!;
const dialogSaveFileDefaultPathInput = document.querySelector<HTMLInputElement>(
  "#dialog-save-file-default-path-input"
)!;
const dialogSaveFileFiltersTextarea =
  document.querySelector<HTMLTextAreaElement>(
    "#dialog-save-file-filters-textarea"
  )!;
const dialogSaveFileInvokeButton = document.querySelector<HTMLButtonElement>(
  "#dialog-save-file-invoke-button"
)!;
const dialogSaveFileInvokeResult = document.querySelector<HTMLSpanElement>(
  "#dialog-save-file-invoke-result"
)!;

dialogSaveFileInvokeButton.addEventListener("click", async () => {
  const title = dialogSaveFileTitleInput.value;
  const defaultPath = dialogSaveFileDefaultPathInput.value;
  const filters = eval(dialogSaveFileFiltersTextarea.value);

  dialogSaveFileInvokeResult.textContent = "";
  try {
    const result = await saveFile({
      title,
      defaultPath,
      filters,
    });
    dialogSaveFileInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    dialogSaveFileInvokeResult.textContent = `error: ${error.message}`;
  }
});
