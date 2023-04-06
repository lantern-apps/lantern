import * as _lantern_app_core from '@lantern-app/core';

declare function show(): Promise<void>;
declare function hide(): Promise<void>;
declare function isVisible(): Promise<boolean>;
declare function setTooltip(tooltip: string): Promise<void>;
declare function setMenu(items: MenuItem[]): Promise<void>;
declare function onClick(): _lantern_app_core.Observable<void>;
declare function offClick(): void;
declare function onMenuItemClick(): _lantern_app_core.Observable<MenuItemClickEvent>;
declare function offMenuItemClick(): void;
interface MenuNormalItem {
    type?: "normal";
    id: string;
    text: string;
    enabled?: boolean;
}
interface MenuSubmenuItem {
    type: "submenu";
    text: string;
    enabled?: boolean;
    items?: MenuItem[];
}
interface MenuSeparatorItem {
    type: "separator";
}
type MenuItem = MenuNormalItem | MenuSubmenuItem | MenuSeparatorItem;
interface MenuItemClickEvent {
    id: string;
}

export { MenuItem, MenuItemClickEvent, MenuNormalItem, MenuSeparatorItem, MenuSubmenuItem, hide, isVisible, offClick, offMenuItemClick, onClick, onMenuItemClick, setMenu, setTooltip, show };
