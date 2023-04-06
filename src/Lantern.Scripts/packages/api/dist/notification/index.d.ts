declare function send(options: SendOptions): Promise<void>;
interface SendOptions {
    title?: string;
    body?: string;
}

export { SendOptions, send };
