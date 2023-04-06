import { getAll } from "@lantern-app/api/screen";

const screenGetAllInvokeButton = document.querySelector<HTMLButtonElement>(
  "#screen-get-all-invoke-button"
)!;
const screenGetAllInvokeResult = document.querySelector<HTMLSpanElement>(
  "#screen-get-all-invoke-result"
)!;

screenGetAllInvokeButton.addEventListener("click", async () => {
  screenGetAllInvokeResult.textContent = "";
  try {
    const result = await getAll();
    screenGetAllInvokeResult.textContent = `success: ${JSON.stringify(result)}`;
  } catch (error: any) {
    screenGetAllInvokeResult.textContent = `error: ${error.message}`;
  }
});
