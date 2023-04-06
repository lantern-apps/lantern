import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
function shutdown() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.app.shutdown");
}
function getVersion() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.app.getVersion");
}
const appGetVersionInvokeButton = document.querySelector(
  "#app-get-version-invoke-button"
);
const appGetVersionInvokeResult = document.querySelector(
  "#app-get-version-invoke-result"
);
appGetVersionInvokeButton.addEventListener("click", async () => {
  appGetVersionInvokeResult.textContent = "";
  try {
    const result = await getVersion();
    appGetVersionInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    appGetVersionInvokeResult.textContent = `error: ${error.message}`;
  }
});
const appShutdownInvokeButton = document.querySelector(
  "#app-shutdown-invoke-button"
);
const appShutdownInvokeResult = document.querySelector(
  "#app-shutdown-invoke-result"
);
appShutdownInvokeButton.addEventListener("click", async () => {
  appShutdownInvokeResult.textContent = "";
  try {
    await shutdown();
    appShutdownInvokeResult.textContent = `success`;
  } catch (error) {
    appShutdownInvokeResult.textContent = `error: ${error.message}`;
  }
});
