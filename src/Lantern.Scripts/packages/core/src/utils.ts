export function uid() {
  return window.crypto.getRandomValues(new Uint16Array(1))[0];
}
