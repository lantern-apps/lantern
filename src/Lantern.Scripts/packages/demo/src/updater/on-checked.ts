import { offChecked, onChecked } from "@lantern-app/api/updater";
import { Subscription } from "@lantern-app/core";

const updaterOnCheckedListenButton = document.querySelector<HTMLButtonElement>(
  "#updater-on-checked-listen-button"
)!;
const updaterOnCheckedUnlistenButton =
  document.querySelector<HTMLButtonElement>(
    "#updater-on-checked-unlisten-button"
  )!;
const updaterOnCheckedListenResult = document.querySelector<HTMLSpanElement>(
  "#updater-on-checked-listen-result"
)!;
let subscription: Subscription | undefined;

updaterOnCheckedListenButton.addEventListener("click", async () => {
  if (subscription) {
    subscription.unsubscribe();
  }

  let times = 0;
  updaterOnCheckedListenResult.textContent = "";
  try {
    subscription = onChecked().subscribe((event) => {
      updaterOnCheckedListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error: any) {
    updaterOnCheckedListenResult.textContent = `error: ${error.message}`;
  }
});

updaterOnCheckedUnlistenButton.addEventListener("click", async () => {
  try {
    offChecked();
  } catch (error: any) {
    updaterOnCheckedListenResult.textContent = `error: ${error.message}`;
  }
});
