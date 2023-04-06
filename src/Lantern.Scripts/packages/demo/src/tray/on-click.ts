import { offClick, onClick } from "@lantern-app/api/tray";
import { Subscription } from "@lantern-app/core";

const trayOnClickListenButton = document.querySelector<HTMLButtonElement>(
  "#tray-on-click-listen-button"
)!;
const trayOnClickUnlistenButton = document.querySelector<HTMLButtonElement>(
  "#tray-on-click-unlisten-button"
)!;
const trayOnClickListenResult = document.querySelector<HTMLSpanElement>(
  "#tray-on-click-listen-result"
)!;
let subscription: Subscription | undefined;

trayOnClickListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  trayOnClickListenResult.textContent = "";
  try {
    subscription = onClick().subscribe((event) => {
      trayOnClickListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error: any) {
    trayOnClickListenResult.textContent = `error: ${error.message}`;
  }
});

trayOnClickUnlistenButton.addEventListener("click", async () => {
  try {
    offClick();
  } catch (error: any) {
    trayOnClickListenResult.textContent = `error: ${error.message}`;
  }
});
