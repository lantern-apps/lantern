declare global {
    interface SymbolConstructor {
        readonly observable: symbol;
    }
}
interface Subscribable<T> {
    subscribe(observer: Partial<Observer<T>>): Unsubscribable;
}
interface Unsubscribable {
    unsubscribe(): void;
}
interface Observable<T> extends Subscribable<T> {
    subscribe(observer: Partial<Observer<T>>): Subscription;
    subscribe(next?: ((value: T) => void) | null, error?: ((error: any) => void) | null, complete?: (() => void) | null): Subscription;
    [Symbol.observable]: () => Subscribable<T>;
}
interface Subscription extends Unsubscribable {
}
interface Observer<T> {
    next: (value: T) => void;
    error: (err: any) => void;
    complete: () => void;
}

interface Ipc {
    invoke: IpcInvoke;
    listen: IpcListen;
    unlisten: IpcUnlisten;
}
interface IpcInvoke {
    <TResult = void>(name: string): Promise<TResult>;
    <TResult = void, TArgs = any>(name: string, args?: TArgs, target?: string): Promise<TResult>;
}
interface IpcListen {
    <TData = void>(name: string, target?: string): Observable<TData>;
}
interface IpcUnlisten {
    (name: string, target?: string): void;
}

declare global {
    interface Window {
        lantern: Lantern;
    }
}
interface Lantern {
    ipc: Ipc;
}

declare const lantern: Lantern;

export { Ipc, IpcInvoke, IpcListen, IpcUnlisten, Lantern, Observable, Observer, Subscribable, Subscription, Unsubscribable, lantern as default };
