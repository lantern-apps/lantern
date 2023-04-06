declare global {
  interface SymbolConstructor {
    readonly observable: symbol;
  }
}

export interface Subscribable<T> {
  subscribe(observer: Partial<Observer<T>>): Unsubscribable;
}

export interface Unsubscribable {
  unsubscribe(): void;
}

export interface Observable<T> extends Subscribable<T> {
  subscribe(observer: Partial<Observer<T>>): Subscription;
  subscribe(
    next?: ((value: T) => void) | null,
    error?: ((error: any) => void) | null,
    complete?: (() => void) | null
  ): Subscription;
  [Symbol.observable]: () => Subscribable<T>;
}

export interface Subscription extends Unsubscribable {}

export interface Observer<T> {
  next: (value: T) => void;
  error: (err: any) => void;
  complete: () => void;
}
