declare function shutdown(): Promise<void>;
declare function getVersion(): Promise<string>;

export { getVersion, shutdown };
