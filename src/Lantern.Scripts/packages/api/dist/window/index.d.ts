import * as _lantern_app_core from '@lantern-app/core';
import { S as Size, P as Position, W as WindowTitleBarStyle } from '../models-d1c109e2.js';
export { R as Rectangle } from '../models-d1c109e2.js';

interface ResizedEvent {
    size: Size;
}
interface MovedEvent {
    position: Position;
}

declare function restore(window?: string): Promise<void>;
declare function maximize(window?: string): Promise<void>;
declare function minimize(window?: string): Promise<void>;
declare function fullScreen(window?: string): Promise<void>;
declare function close(window?: string): Promise<void>;
declare function hide(window?: string): Promise<void>;
declare function show(window?: string): Promise<void>;
declare function activate(window?: string): Promise<void>;
declare function center(window?: string): Promise<void>;
declare function setAlwaysOnTop(alwaysOnTop: boolean, window?: string): Promise<void>;
declare function setSize(width: number, height: number, window?: string): Promise<void>;
declare function setPosition(x: number, y: number, window?: string): Promise<void>;
declare function setTitle(title: string, window?: string): Promise<void>;
declare function setUrl(url: string, window?: string): Promise<void>;
declare function setMinSize(width: number, height: number, window?: string): Promise<void>;
declare function setMaxSize(width: number, height: number, window?: string): Promise<void>;
declare function setSkipTaskbar(skipTaskbar: boolean, window?: string): Promise<void>;
declare function setResizable(resizable: boolean, window?: string): Promise<void>;
declare function setTitleBarStyle(titleBarStyle: WindowTitleBarStyle, window?: string): Promise<void>;
declare function isMaximized(window?: string): Promise<boolean>;
declare function isMinimized(window?: string): Promise<boolean>;
declare function isFullScreen(window?: string): Promise<boolean>;
declare function isAlwaysOnTop(window?: string): Promise<boolean>;
declare function isVisible(window?: string): Promise<boolean>;
declare function isActive(window?: string): Promise<boolean>;
declare function isSkipTaskbar(window?: string): Promise<boolean>;
declare function isResizable(window?: string): Promise<boolean>;
declare function getTitle(window?: string): Promise<string | null>;
declare function getUrl(window?: string): Promise<string | null>;
declare function getSize(window?: string): Promise<Size>;
declare function getPosition(window?: string): Promise<Position>;
declare function getMinSize(window?: string): Promise<Size>;
declare function getMaxSize(window?: string): Promise<Size>;
declare function getTitleBarStyle(window?: string): Promise<WindowTitleBarStyle>;
declare function onClosing(window?: string): _lantern_app_core.Observable<void>;
declare function offClosing(window?: string): void;
declare function onClosed(window?: string): _lantern_app_core.Observable<void>;
declare function offClosed(window?: string): void;
declare function onResized(window?: string): _lantern_app_core.Observable<ResizedEvent>;
declare function offResized(window?: string): void;
declare function onMoved(window?: string): _lantern_app_core.Observable<MovedEvent>;
declare function offMoved(window?: string): void;

interface CreateOptions {
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

declare class Window {
    #private;
    get name(): string | undefined;
    constructor(name?: string);
    restore(): Promise<void>;
    maximize(): Promise<void>;
    minimize(): Promise<void>;
    fullScreen(): Promise<void>;
    close(): Promise<void>;
    hide(): Promise<void>;
    show(): Promise<void>;
    activate(): Promise<void>;
    center(): Promise<void>;
    setAlwaysOnTop(alwaysOnTop: boolean): Promise<void>;
    setSize(width: number, height: number): Promise<void>;
    setPosition(x: number, y: number): Promise<void>;
    setTitle(title: string): Promise<void>;
    setUrl(url: string): Promise<void>;
    setMinSize(width: number, height: number): Promise<void>;
    setMaxSize(width: number, height: number): Promise<void>;
    setSkipTaskbar(skipTaskbar: boolean): Promise<void>;
    setResizable(resizable: boolean): Promise<void>;
    setTitleBarStyle(titleBarStyle: WindowTitleBarStyle): Promise<void>;
    isMaximized(): Promise<boolean>;
    isMinimized(): Promise<boolean>;
    isFullScreen(): Promise<boolean>;
    isAlwaysOnTop(): Promise<boolean>;
    isVisible(): Promise<boolean>;
    isActive(): Promise<boolean>;
    isSkipTaskbar(): Promise<boolean>;
    isResizable(): Promise<boolean>;
    getTitle(): Promise<string | null>;
    getUrl(): Promise<string | null>;
    getSize(): Promise<Size>;
    getPosition(): Promise<Position>;
    getMinSize(): Promise<Size>;
    getMaxSize(): Promise<Size>;
    getTitleBarStyle(): Promise<WindowTitleBarStyle>;
    onClosing(): _lantern_app_core.Observable<void>;
    offClosing(): void;
    onClosed(): _lantern_app_core.Observable<void>;
    offClosed(): void;
    onResized(): _lantern_app_core.Observable<ResizedEvent>;
    offResized(): void;
    onMoved(): _lantern_app_core.Observable<MovedEvent>;
    offMoved(): void;
}

declare function getAll(): Promise<Window[]>;
declare function getCurrent(): Promise<Window>;
declare function create(options: CreateOptions): Promise<Window>;

export { CreateOptions, MovedEvent, Position, ResizedEvent, Size, Window, WindowTitleBarStyle, activate, center, close, create, fullScreen, getAll, getCurrent, getMaxSize, getMinSize, getPosition, getSize, getTitle, getTitleBarStyle, getUrl, hide, isActive, isAlwaysOnTop, isFullScreen, isMaximized, isMinimized, isResizable, isSkipTaskbar, isVisible, maximize, minimize, offClosed, offClosing, offMoved, offResized, onClosed, onClosing, onMoved, onResized, restore, setAlwaysOnTop, setMaxSize, setMinSize, setPosition, setResizable, setSize, setSkipTaskbar, setTitle, setTitleBarStyle, setUrl, show };
