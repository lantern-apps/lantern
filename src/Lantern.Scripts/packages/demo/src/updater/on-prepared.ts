import { offPrepared, onPrepared } from "@lantern-app/api/updater";
import { Subscription } from "@lantern-app/core";

const updaterOnPreparedListenButton = document.querySelector<HTMLButtonElement>(
  "#updater-on-prepared-listen-button"
)!;
const updaterOnPreparedUnlistenButton =
  document.querySelector<HTMLButtonElement>(
    "#updater-on-prepared-unlisten-button"
  )!;
const updaterOnPreparedListenResult = document.querySelector<HTMLSpanElement>(
  "#updater-on-prepared-listen-result"
)!;
let subscription: Subscription | undefined;

updaterOnPreparedListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  updaterOnPreparedListenResult.textContent = "";
  try {
    subscription = onPrepared().subscribe((event) => {
      updaterOnPreparedListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error: any) {
    updaterOnPreparedListenResult.textContent = `error: ${error.message}`;
  }
});

updaterOnPreparedUnlistenButton.addEventListener("click", async () => {
  try {
    offPrepared();
  } catch (error: any) {
    updaterOnPreparedListenResult.textContent = `error: ${error.message}`;
  }
});
