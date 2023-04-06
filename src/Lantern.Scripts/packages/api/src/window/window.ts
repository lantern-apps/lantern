import {
  restore,
  maximize,
  minimize,
  fullScreen,
  close,
  hide,
  show,
  activate,
  center,
  setAlwaysOnTop,
  setSize,
  setPosition,
  setTitle,
  setUrl,
  setMinSize,
  setMaxSize,
  isMaximized,
  isMinimized,
  isFullScreen,
  isAlwaysOnTop,
  isVisible,
  isActive,
  getTitle,
  getUrl,
  getSize,
  getPosition,
  getMinSize,
  getMaxSize,
  onClosing,
  offClosing,
  onClosed,
  offClosed,
  onResized,
  offResized,
  onMoved,
  offMoved,
  getTitleBarStyle,
  isSkipTaskbar,
  isResizable,
  setSkipTaskbar,
  setResizable,
  setTitleBarStyle,
} from "./actions";
import { WindowTitleBarStyle } from "../models";

export class Window {
  #name?: string;

  get name() {
    return this.#name;
  }

  constructor(name?: string) {
    this.#name = name || undefined;
  }

  restore() {
    return restore(this.name);
  }

  maximize() {
    return maximize(this.name);
  }

  minimize() {
    return minimize(this.name);
  }

  fullScreen() {
    return fullScreen(this.name);
  }

  close() {
    return close(this.name);
  }

  hide() {
    return hide(this.name);
  }

  show() {
    return show(this.name);
  }

  activate() {
    return activate(this.name);
  }

  center() {
    return center(this.name);
  }

  setAlwaysOnTop(alwaysOnTop: boolean) {
    return setAlwaysOnTop(alwaysOnTop, this.name);
  }

  setSize(width: number, height: number) {
    return setSize(width, height, this.name);
  }

  setPosition(x: number, y: number) {
    return setPosition(x, y, this.name);
  }

  setTitle(title: string) {
    return setTitle(title, this.name);
  }

  setUrl(url: string) {
    return setUrl(url, this.name);
  }

  setMinSize(width: number, height: number) {
    return setMinSize(width, height, this.name);
  }

  setMaxSize(width: number, height: number) {
    return setMaxSize(width, height, this.name);
  }

  setSkipTaskbar(skipTaskbar: boolean) {
    return setSkipTaskbar(skipTaskbar, this.name);
  }

  setResizable(resizable: boolean) {
    return setResizable(resizable, this.name);
  }

  setTitleBarStyle(titleBarStyle: WindowTitleBarStyle) {
    return setTitleBarStyle(titleBarStyle, this.name);
  }

  isMaximized() {
    return isMaximized(this.name);
  }

  isMinimized() {
    return isMinimized(this.name);
  }

  isFullScreen() {
    return isFullScreen(this.name);
  }

  isAlwaysOnTop() {
    return isAlwaysOnTop(this.name);
  }

  isVisible() {
    return isVisible(this.name);
  }

  isActive() {
    return isActive(this.name);
  }

  isSkipTaskbar() {
    return isSkipTaskbar(this.name);
  }

  isResizable() {
    return isResizable(this.name);
  }

  getTitle() {
    return getTitle(this.name);
  }

  getUrl() {
    return getUrl(this.name);
  }

  getSize() {
    return getSize(this.name);
  }

  getPosition() {
    return getPosition(this.name);
  }

  getMinSize() {
    return getMinSize(this.name);
  }

  getMaxSize() {
    return getMaxSize(this.name);
  }

  getTitleBarStyle() {
    return getTitleBarStyle(this.name);
  }

  onClosing() {
    return onClosing(this.name);
  }

  offClosing() {
    return offClosing(this.name);
  }

  onClosed() {
    return onClosed(this.name);
  }

  offClosed() {
    return offClosed(this.name);
  }

  onResized() {
    return onResized(this.name);
  }

  offResized() {
    return offResized(this.name);
  }

  onMoved() {
    return onMoved(this.name);
  }

  offMoved() {
    return offMoved(this.name);
  }
}
