import { create, WindowTitleBarStyle } from "@lantern-app/api/window";

const windowCreateNameInput = document.querySelector<HTMLInputElement>(
  "#window-create-name-input"
)!;
const windowCreateTitleInput = document.querySelector<HTMLInputElement>(
  "#window-create-title-input"
)!;
const windowCreateUrlInput = document.querySelector<HTMLInputElement>(
  "#window-create-url-input"
)!;
const windowCreateWidthInput = document.querySelector<HTMLInputElement>(
  "#window-create-width-input"
)!;
const windowCreateHeightInput = document.querySelector<HTMLInputElement>(
  "#window-create-height-input"
)!;
const windowCreateXInput = document.querySelector<HTMLInputElement>(
  "#window-create-x-input"
)!;
const windowCreateYInput = document.querySelector<HTMLInputElement>(
  "#window-create-y-input"
)!;
const windowCreateMinWidthInput = document.querySelector<HTMLInputElement>(
  "#window-create-min-width-input"
)!;
const windowCreateMinHeightInput = document.querySelector<HTMLInputElement>(
  "#window-create-min-height-input"
)!;
const windowCreateMaxWidthInput = document.querySelector<HTMLInputElement>(
  "#window-create-max-width-input"
)!;
const windowCreateMaxHeightInput = document.querySelector<HTMLInputElement>(
  "#window-create-max-height-input"
)!;
const windowCreateInvokeButton = document.querySelector<HTMLButtonElement>(
  "#window-create-invoke-button"
)!;
const windowCreateInvokeResult = document.querySelector<HTMLSpanElement>(
  "#window-create-invoke-result"
)!;
const windowCreateVisibleInput = document.querySelector<HTMLInputElement>(
  "#window-create-visible-input"
)!;
const windowCreateCenterInput = document.querySelector<HTMLInputElement>(
  "#window-create-center-input"
)!;
const windowCreateAlwaysOnTopInput = document.querySelector<HTMLInputElement>(
  "#window-create-always-on-top-input"
)!;
const windowCreateSkipTaskbarInput = document.querySelector<HTMLInputElement>(
  "#window-create-skip-taskbar-input"
)!;
const windowCreateResizableInput = document.querySelector<HTMLInputElement>(
  "#window-create-resizable-input"
)!;
const windowCreateTitleBarStyleSelect =
  document.querySelector<HTMLSelectElement>(
    "#window-create-title-bar-style-select"
  )!;

windowCreateInvokeButton.addEventListener("click", async () => {
  const name = windowCreateNameInput.value;
  const title = windowCreateTitleInput.value;
  const url = windowCreateUrlInput.value;
  const width = windowCreateWidthInput.valueAsNumber;
  const height = windowCreateHeightInput.valueAsNumber;
  const x = windowCreateXInput.valueAsNumber;
  const y = windowCreateYInput.valueAsNumber;
  const minWidth = windowCreateMinWidthInput.valueAsNumber;
  const minHeight = windowCreateMinHeightInput.valueAsNumber;
  const maxWidth = windowCreateMaxWidthInput.valueAsNumber;
  const maxHeight = windowCreateMaxHeightInput.valueAsNumber;
  const visible = windowCreateVisibleInput.checked;
  const center = windowCreateCenterInput.checked;
  const alwaysOnTop = windowCreateAlwaysOnTopInput.checked;
  const skipTaskbar = windowCreateSkipTaskbarInput.checked;
  const resizable = windowCreateResizableInput.checked;
  const titleBarStyle =
    windowCreateTitleBarStyleSelect.value as WindowTitleBarStyle;

  windowCreateInvokeResult.textContent = "";
  try {
    await create({
      name,
      title,
      url,
      width,
      height,
      x,
      y,
      minWidth,
      minHeight,
      maxWidth,
      maxHeight,
      visible,
      center,
      alwaysOnTop,
      skipTaskbar,
      resizable,
      titleBarStyle,
    });
    windowCreateInvokeResult.textContent = `success`;
  } catch (error: any) {
    windowCreateInvokeResult.textContent = `error: ${error.message}`;
  }
});
