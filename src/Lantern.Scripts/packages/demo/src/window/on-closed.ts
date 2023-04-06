import { Subscription } from "@lantern-app/core";

import { getSelectedWindow } from "./select-window";

const windowOnClosedListenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-closed-listen-button"
)!;
const windowOnClosedUnlistenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-closed-unlisten-button"
)!;
const windowOnClosedListenResult = document.querySelector<HTMLSpanElement>(
  "#window-on-closed-listen-result"
)!;
let subscription: Subscription | undefined;

windowOnClosedListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  windowOnClosedListenResult.textContent = "";
  try {
    subscription = getSelectedWindow()
      .onClosed()
      .subscribe(() => {
        windowOnClosedListenResult.textContent = `trigger ${++times} times`;
      });
  } catch (error: any) {
    windowOnClosedListenResult.textContent = `error: ${error.message}`;
  }
});

windowOnClosedUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offClosed();
  } catch (error: any) {
    windowOnClosedListenResult.textContent = `error: ${error.message}`;
  }
});
