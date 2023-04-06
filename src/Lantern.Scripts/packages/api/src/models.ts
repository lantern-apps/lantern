export interface Size {
  width: number;
  height: string;
}

export interface Position {
  x: number;
  y: string;
}

export interface Rectangle extends Size, Position {}

export type WindowTitleBarStyle = "default" | "none";
