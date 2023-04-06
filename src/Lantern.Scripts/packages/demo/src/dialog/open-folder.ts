import { openFolder } from "@lantern-app/api/dialog";

const dialogOpenFolderTitleInput = document.querySelector<HTMLInputElement>(
  "#dialog-open-folder-title-input"
)!;
const dialogOpenFolderDefaultPathInput =
  document.querySelector<HTMLInputElement>(
    "#dialog-open-folder-default-path-input"
  )!;
const dialogOpenFolderMultipleInput = document.querySelector<HTMLInputElement>(
  "#dialog-open-folder-multiple-input"
)!;
const dialogOpenFolderInvokeButton = document.querySelector<HTMLButtonElement>(
  "#dialog-open-folder-invoke-button"
)!;
const dialogOpenFolderInvokeResult = document.querySelector<HTMLSpanElement>(
  "#dialog-open-folder-invoke-result"
)!;

dialogOpenFolderInvokeButton.addEventListener("click", async () => {
  const title = dialogOpenFolderTitleInput.value;
  const defaultPath = dialogOpenFolderDefaultPathInput.value;
  const multiple = dialogOpenFolderMultipleInput.checked;

  dialogOpenFolderInvokeResult.textContent = "";
  try {
    const result = await openFolder({
      title,
      defaultPath,
      multiple,
    });
    dialogOpenFolderInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error: any) {
    dialogOpenFolderInvokeResult.textContent = `error: ${error.message}`;
  }
});
