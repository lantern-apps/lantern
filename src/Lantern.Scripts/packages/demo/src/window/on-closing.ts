import { Subscription } from "@lantern-app/core";

import { getSelectedWindow } from "./select-window";

const windowOnClosingListenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-closing-listen-button"
)!;
const windowOnClosingUnlistenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-closing-unlisten-button"
)!;
const windowOnClosingListenResult = document.querySelector<HTMLSpanElement>(
  "#window-on-closing-listen-result"
)!;
let subscription: Subscription | undefined;

windowOnClosingListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  windowOnClosingListenResult.textContent = "";
  try {
    subscription = getSelectedWindow()
      .onClosing()
      .subscribe(() => {
        windowOnClosingListenResult.textContent = `trigger ${++times} times`;
      });
  } catch (error: any) {
    windowOnClosingListenResult.textContent = `error: ${error.message}`;
  }
});

windowOnClosingUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offClosing();
  } catch (error: any) {
    windowOnClosingListenResult.textContent = `error: ${error.message}`;
  }
});
