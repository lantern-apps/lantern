import { Webview } from "./webview";

declare global {
  interface Window {
    chrome: Chrome;
  }
}

export interface Chrome {
  webview: Webview;
}

const chrome: Chrome = window.chrome;

export default chrome;
