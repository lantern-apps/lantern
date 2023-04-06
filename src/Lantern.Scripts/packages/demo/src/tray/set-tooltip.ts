import { setTooltip } from "@lantern-app/api/tray";

const traySetTooltipTooltipInput = document.querySelector<HTMLInputElement>(
  "#tray-set-tooltip-tooltip-input"
)!;
const traySetTooltipInvokeButton = document.querySelector<HTMLButtonElement>(
  "#tray-set-tooltip-invoke-button"
)!;
const traySetTooltipInvokeResult = document.querySelector<HTMLSpanElement>(
  "#tray-set-tooltip-invoke-result"
)!;

traySetTooltipInvokeButton.addEventListener("click", async () => {
  const tooltip = traySetTooltipTooltipInput.value;

  traySetTooltipInvokeResult.textContent = "";
  try {
    await setTooltip(tooltip);
    traySetTooltipInvokeResult.textContent = `success`;
  } catch (error: any) {
    traySetTooltipInvokeResult.textContent = `error: ${error.message}`;
  }
});
