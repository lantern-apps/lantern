import * as _lantern_app_core from '@lantern-app/core';

declare function launch(): Promise<void>;
declare function perform(): Promise<void>;
declare function check(): Promise<UpdateInfo>;
declare function onPrepared(): _lantern_app_core.Observable<PreparedEvent>;
declare function offPrepared(): void;
declare function onChecked(): _lantern_app_core.Observable<CheckedEvent>;
declare function offChecked(): _lantern_app_core.Observable<void>;
interface UpdateInfo {
    version: string;
    size: string;
}
interface PreparedEvent {
    version: string;
    size: string;
}
interface CheckedEvent {
    version: string;
    size: string;
}

export { CheckedEvent, PreparedEvent, UpdateInfo, check, launch, offChecked, offPrepared, onChecked, onPrepared, perform };
