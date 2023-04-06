import { Subscription } from "@lantern-app/core";

import { getSelectedWindow } from "./select-window";

const windowOnResizedListenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-resized-listen-button"
)!;
const windowOnResizedUnlistenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-resized-unlisten-button"
)!;
const windowOnResizedListenResult = document.querySelector<HTMLSpanElement>(
  "#window-on-resized-listen-result"
)!;
let subscription: Subscription | undefined;

windowOnResizedListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  windowOnResizedListenResult.textContent = "";
  try {
    subscription = getSelectedWindow()
      .onResized()
      .subscribe((event) => {
        windowOnResizedListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
          event
        )}`;
      });
  } catch (error: any) {
    windowOnResizedListenResult.textContent = `error: ${error.message}`;
  }
});

windowOnResizedUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offResized();
  } catch (error: any) {
    windowOnResizedListenResult.textContent = `error: ${error.message}`;
  }
});
