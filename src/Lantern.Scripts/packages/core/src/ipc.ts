import ZObservable from "zen-observable";

import { Observable } from "./observable";
import { ObserverKey, ObserverManager } from "./observer-manager";
import { uid } from "./utils";
import webview, { WebviewEvent } from "./webview";

const observerManager = new ObserverManager();

webview.addEventListener("message", webviewMessageListener);

function webviewMessageListener(event: WebviewEvent<IpcEvent | IpcResponse>) {
  const message = event.data;
  if (!message) {
    return;
  }

  if ("id" in message) {
    processInvokeObserver(message);
  } else {
    processListenObserver(message);
  }
}

function processInvokeObserver(request: IpcResponse) {
  const key: ObserverKey = {
    id: request.id,
  };

  const observer = observerManager.find(key);
  if (!observer) {
    return;
  }

  if (request.error) {
    observer.error(new Error(request.body));
  } else {
    observer.next(request.body);
    observer.complete();
  }

  observerManager.remove(key);
}

function processListenObserver(event: IpcEvent) {
  const key: ObserverKey = {
    name: event.name,
    target: event.target,
  };

  const observers = observerManager.filter(key);

  observers.forEach((observer) => {
    observer.next(event.body);
  });
}

const invoke: IpcInvoke = (
  name: string,
  args?: any,
  target?: string
): Promise<any> => {
  return new Promise((resolve, reject) => {
    new ZObservable((observer) => {
      const id = uid();
      const key: ObserverKey = {
        id,
      };

      observerManager.add(key, observer);

      const message: IpcRequest = {
        name,
        id,
        body: args,
        target,
      };

      webview.postMessage(message);
    }).subscribe(resolve, reject);
  });
};

const listen: IpcListen = (name: string, target?: string): Observable<any> => {
  return new ZObservable((observer) => {
    const key: ObserverKey = {
      name,
      target,
    };

    if (!observerManager.any(key)) {
      webview.postMessage({
        name: "lantern.event.listen",
        target,
        body: {
          event: name,
        },
      });
    }

    const exactKey: ObserverKey = {
      id: uid(),
      name,
      target: target,
    };
    observerManager.add(exactKey, observer);

    return () => {
      observer.complete();

      observerManager.remove(exactKey);

      if (!observerManager.any(key)) {
        webview.postMessage({
          name: "lantern.event.unlisten",
          target,
          body: {
            event: name,
          },
        });
      }
    };
  });
};

const unlisten: IpcUnlisten = (name: string, target?: string) => {
  const key: ObserverKey = {
    name,
    target,
  };

  if (!observerManager.any(key)) {
    return;
  }

  webview.postMessage({
    name: "lantern.event.unlisten",
    target,
    body: {
      event: name,
    },
  });

  observerManager.filter(key).forEach((observer) => {
    observer.complete();
  });

  observerManager.remove(key);
};

interface IpcResponse {
  id: number;

  body?: any;

  error?: boolean;
}

interface IpcRequest {
  name: string;

  id: number;

  body?: any;

  target?: string;
}

interface IpcEvent {
  name: string;

  body?: any;

  target?: string;
}

export const ipc: Ipc = {
  invoke,
  listen,
  unlisten,
};

export interface Ipc {
  invoke: IpcInvoke;
  listen: IpcListen;
  unlisten: IpcUnlisten;
}

export interface IpcInvoke {
  <TResult = void>(name: string): Promise<TResult>;
  <TResult = void, TArgs = any>(
    name: string,
    args?: TArgs,
    target?: string
  ): Promise<TResult>;
}

export interface IpcListen {
  <TData = void>(name: string, target?: string): Observable<TData>;
}

export interface IpcUnlisten {
  (name: string, target?: string): void;
}
