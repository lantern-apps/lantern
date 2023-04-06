import { getAll, Window } from "@lantern-app/api/window";

import { createOptionElement } from "../utils";

const windowSelectWindowSelect = document.querySelector<HTMLSelectElement>(
  "#window-select-window-select"
)!;
const windowSelectRefreshButton = document.querySelector<HTMLButtonElement>(
  "#window-select-refresh-button"
)!;
const windowSelectRefreshResult = document.querySelector<HTMLSpanElement>(
  "#window-select-refresh-result"
)!;

windowSelectRefreshButton.addEventListener("click", async () => {
  windowSelectRefreshResult.textContent = "";
  try {
    const result = await getAll();
    windowSelectRefreshResult.textContent = `success`;

    windowSelectWindowSelect.replaceChildren(
      createOptionElement("", "current"),
      ...result.map((window) => createOptionElement(window.name, window.name))
    );
  } catch (error: any) {
    windowSelectRefreshResult.textContent = `error: ${error.message}`;
  }
});

export function getSelectedWindow() {
  return new Window(windowSelectWindowSelect.value);
}
