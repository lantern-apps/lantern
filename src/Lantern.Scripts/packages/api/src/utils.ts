import lantern from "@lantern-app/core";

export function throwErrorIfNoLantern() {
  if (!lantern) {
    throw new Error("lantern is not exists");
  }
}
