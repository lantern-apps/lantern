import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
function launch() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.updater.launch");
}
function perform() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.updater.perform");
}
function check() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.updater.check");
}
function onPrepared() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.updater.onPrepared");
}
function offPrepared() {
  throwErrorIfNoLantern();
  return lantern.ipc.unlisten("lantern.updater.onPrepared");
}
function onChecked() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.updater.onChecked");
}
function offChecked() {
  throwErrorIfNoLantern();
  return lantern.ipc.listen("lantern.updater.onChecked");
}
const updaterCheckInvokeButton = document.querySelector(
  "#updater-check-invoke-button"
);
const updaterCheckInvokeResult = document.querySelector(
  "#updater-check-invoke-result"
);
updaterCheckInvokeButton.addEventListener("click", async () => {
  updaterCheckInvokeResult.textContent = "";
  try {
    const result = await check();
    updaterCheckInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error) {
    updaterCheckInvokeResult.textContent = `error: ${error.message}`;
  }
});
const updaterLaunchInvokeButton = document.querySelector(
  "#updater-launch-invoke-button"
);
const updaterLaunchInvokeResult = document.querySelector(
  "#updater-launch-invoke-result"
);
updaterLaunchInvokeButton.addEventListener("click", async () => {
  updaterLaunchInvokeResult.textContent = "";
  try {
    await launch();
    updaterLaunchInvokeResult.textContent = `success`;
  } catch (error) {
    updaterLaunchInvokeResult.textContent = `error: ${error.message}`;
  }
});
const updaterOnCheckedListenButton = document.querySelector(
  "#updater-on-checked-listen-button"
);
const updaterOnCheckedUnlistenButton = document.querySelector(
  "#updater-on-checked-unlisten-button"
);
const updaterOnCheckedListenResult = document.querySelector(
  "#updater-on-checked-listen-result"
);
let subscription$1;
updaterOnCheckedListenButton.addEventListener("click", async () => {
  if (subscription$1) {
    subscription$1.unsubscribe();
  }
  let times = 0;
  updaterOnCheckedListenResult.textContent = "";
  try {
    subscription$1 = onChecked().subscribe((event) => {
      updaterOnCheckedListenResult.textContent = `trigger ${++times} times: ${JSON.stringify(
        event
      )}`;
    });
  } catch (error) {
    updaterOnCheckedListenResult.textContent = `error: ${error.message}`;
  }
});
updaterOnCheckedUnlistenButton.addEventListener("click", async () => {
  try {
    offChecked();
  } catch (error) {
    updaterOnCheckedListenResult.textContent = `error: ${error.message}`;
  }
});
const updaterOnPreparedListenButton = document.querySelector(
  "#updater-on-prepared-listen-button"
);
const updaterOnPreparedUnlistenButton = document.querySelector(
  "#updater-on-prepared-unlisten-button"
);
const updaterOnPreparedListenResult = document.querySelector(
  "#updater-on-prepared-listen-result"
);
let subscription;
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
  } catch (error) {
    updaterOnPreparedListenResult.textContent = `error: ${error.message}`;
  }
});
updaterOnPreparedUnlistenButton.addEventListener("click", async () => {
  try {
    offPrepared();
  } catch (error) {
    updaterOnPreparedListenResult.textContent = `error: ${error.message}`;
  }
});
const updaterPerformInvokeButton = document.querySelector(
  "#updater-perform-invoke-button"
);
const updaterPerformInvokeResult = document.querySelector(
  "#updater-perform-invoke-result"
);
updaterPerformInvokeButton.addEventListener("click", async () => {
  updaterPerformInvokeResult.textContent = "";
  try {
    await perform();
    updaterPerformInvokeResult.textContent = `success`;
  } catch (error) {
    updaterPerformInvokeResult.textContent = `error: ${error.message}`;
  }
});
