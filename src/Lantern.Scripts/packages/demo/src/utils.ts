export function createOptionElement(value?: string, text?: string) {
  const option = document.createElement("option");
  option.value = value || "";
  option.text = text || "";
  return option;
}
