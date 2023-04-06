import { offMenuItemClick, onMenuItemClick } from "@lantern-app/api/tray";
import { Subscription } from "@lantern-app/core";

const trayOnMenuItemClickListenButton =
  document.querySelector<HTMLButtonElement>(
    "#tray-on-menu-item-click-listen-button"
  )!;
const trayOnMenuItemClickUnlistenButton =
  document.querySelector<HTMLButtonElement>(
    "#tray-on-menu-item-click-unlisten-button"
  )!;
const trayOnMenuItemClickListenResult = document.querySelector<HTMLSpanElement>(
  "#tray-on-menu-item-click-listen-result"
)!;
let subscription: Subscription | undefined;

trayOnMenuItemClickListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  trayOnMenuItemClickListenResult.textContent = "";
  try {
    subscription = onMenuItemClick().subscribe((event) => {
      trayOnMenuItemClickListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error: any) {
    trayOnMenuItemClickListenResult.textContent = `error: ${error.message}`;
  }
});

trayOnMenuItemClickUnlistenButton.addEventListener("click", async () => {
  try {
    offMenuItemClick();
  } catch (error: any) {
    trayOnMenuItemClickListenResult.textContent = `error: ${error.message}`;
  }
});
