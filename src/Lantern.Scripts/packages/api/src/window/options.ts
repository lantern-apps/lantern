import { WindowTitleBarStyle } from "../models";

export interface CreateOptions {
  name: string;
  url?: string;
  title?: string;
  width?: number;
  height?: number;
  x?: number;
  y?: number;
  minWidth?: number;
  minHeight?: number;
  maxWidth?: number;
  maxHeight?: number;
  visible?: boolean;
  center?: boolean;
  alwaysOnTop?: boolean;
  skipTaskbar?: boolean;
  resizable?: boolean;
  titleBarStyle?: WindowTitleBarStyle;
}
