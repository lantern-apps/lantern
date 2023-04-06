import chrome from "./chrome";

export interface Webview {
  postMessage<T>(message: T): void;

  addEventListener<T>(
    name: string,
    callback: (event: WebviewEvent<T>) => void
  ): void;

  removeEventListener<T>(
    name: string,
    callback?: (event: WebviewEvent<T>) => void
  ): void;
}

export interface WebviewEvent<T> {
  data?: T;
}

const webview = chrome.webview;

export default webview;
