import { t as throwErrorIfNoLantern, l as lantern } from "../chunk-SX5Y3YHK.js";
function getAll() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.screen.getAll");
}
function getCurrent() {
  throwErrorIfNoLantern();
  return lantern.ipc.invoke("lantern.screen.getCurrent");
}
const screenGetAllInvokeButton = document.querySelector(
  "#screen-get-all-invoke-button"
);
const screenGetAllInvokeResult = document.querySelector(
  "#screen-get-all-invoke-result"
);
screenGetAllInvokeButton.addEventListener("click", async () => {
  screenGetAllInvokeResult.textContent = "";
  try {
    const result = await getAll();
    screenGetAllInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error) {
    screenGetAllInvokeResult.textContent = `error: ${error.message}`;
  }
});
const screenGetCurrentInvokeButton = document.querySelector(
  "#screen-get-current-invoke-button"
);
const screenGetCurrentInvokeResult = document.querySelector(
  "#screen-get-current-invoke-result"
);
screenGetCurrentInvokeButton.addEventListener("click", async () => {
  screenGetCurrentInvokeResult.textContent = "";
  try {
    const result = await getCurrent();
    screenGetCurrentInvokeResult.textContent = `success: ${JSON.stringify(
      result
    )}`;
  } catch (error) {
    screenGetCurrentInvokeResult.textContent = `error: ${error.message}`;
  }
});
