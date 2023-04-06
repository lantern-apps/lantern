import { Subscription } from "@lantern-app/core";

import { getSelectedWindow } from "./select-window";

const windowOnMovedListenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-moved-listen-button"
)!;
const windowOnMovedUnlistenButton = document.querySelector<HTMLButtonElement>(
  "#window-on-moved-unlisten-button"
)!;
const windowOnMovedListenResult = document.querySelector<HTMLSpanElement>(
  "#window-on-moved-listen-result"
)!;
let subscription: Subscription | undefined;

windowOnMovedListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  windowOnMovedListenResult.textContent = "";
  try {
    subscription = getSelectedWindow()
      .onMoved()
      .subscribe((event) => {
        windowOnMovedListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
          event
        )}`;
      });
  } catch (error: any) {
    windowOnMovedListenResult.textContent = `error: ${error.message}`;
  }
});

windowOnMovedUnlistenButton.addEventListener("click", async () => {
  try {
    getSelectedWindow().offMoved();
  } catch (error: any) {
    windowOnMovedListenResult.textContent = `error: ${error.message}`;
  }
});
