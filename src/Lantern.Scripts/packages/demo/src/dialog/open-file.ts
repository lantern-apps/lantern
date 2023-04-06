import { openFile } from "@lantern-app/api/dialog";

const dialogOpenFileTitleInput = document.querySelector<HTMLInputElement>(
  "#dialog-open-file-title-input"
)!;
const dialogOpenFileDefaultPathInput = document.querySelector<HTMLInputElement>(
  "#dialog-open-file-default-path-input"
)!;
const dialogOpenFileFiltersTextarea =
  document.querySelector<HTMLTextAreaElement>(
    "#dialog-open-file-filters-textarea"
  )!;
const dialogOpenFileMultipleInput = document.querySelector<HTMLInputElement>(
  "#dialog-open-file-multiple-input"
)!;
const dialogOpenFileInvokeButton = document.querySelector<HTMLButtonElement>(
  "#dialog-open-file-invoke-button"
)!;
const dialogOpenFileInvokeResult = document.querySelector<HTMLSpanElement>(
  "#dialog-open-file-invoke-result"
)!;

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
      multiple,
    });
    dialogOpenFileInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    dialogOpenFileInvokeResult.textContent = `error: ${error.message}`;
  }
});
