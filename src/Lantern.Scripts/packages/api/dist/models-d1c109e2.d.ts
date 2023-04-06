interface Size {
    width: number;
    height: string;
}
interface Position {
    x: number;
    y: string;
}
interface Rectangle extends Size, Position {
}
type WindowTitleBarStyle = "default" | "none";

export { Position as P, Rectangle as R, Size as S, WindowTitleBarStyle as W };
