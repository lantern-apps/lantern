import { Observer } from "./observable";

// key: id
// key: target_id
// key: target_name_id
export class ObserverManager {
  #observers: Record<string, Observer<any>> = {};

  filter(key: ObserverKey) {
    const stringKey = this.toStringKey(key);
    return Object.entries(this.#observers)
      .filter(([key]) => key.startsWith(stringKey))
      .map(([_, observer]) => observer);
  }

  find(key: ObserverKey) {
    const observers = this.filter(key);
    return observers.length ? observers[0] : undefined;
  }

  add(key: ObserverKey, observer: Observer<any>) {
    const stringKey = this.toStringKey(key);
    this.#observers = {
      ...this.#observers,
      [stringKey]: observer,
    };
  }

  remove(key: ObserverKey) {
    const stringKey = this.toStringKey(key);
    this.#observers = Object.entries(this.#observers).reduce(
      (observers, [key, observer]) =>
        key.startsWith(stringKey)
          ? observers
          : {
              ...observers,
              [key]: observer,
            },
      {}
    );
  }

  any(key: ObserverKey) {
    const stringKey = this.toStringKey(key);
    return !!this.filter(key).length;
  }

  private toStringKey(key: ObserverKey) {
    let stringKey = "";

    if (key.id) {
      stringKey = key.id.toString();
    }

    if (key.name) {
      stringKey = `${key.name}_${stringKey}`;
    }

    if (key.target) {
      stringKey = `${key.target}_${stringKey}`;
    }

    return stringKey;
  }
}

export interface ObserverKey {
  id?: number;
  name?: string;
  target?: string;
}
