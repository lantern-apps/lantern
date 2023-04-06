declare function message(options: MessageOptions): Promise<void>;
declare function ask(options: AskOptions): Promise<boolean>;
declare function confirm(options: ConfirmOptions): Promise<boolean>;
declare function openFile(options: OpenFileOptions): Promise<string[]>;
declare function openFolder(options: OpenFolderOptions): Promise<string[]>;
declare function saveFile(options: SaveFileOptions): Promise<string | null>;
interface MessageOptions {
    title?: string;
    body?: string;
}
interface AskOptions {
    title?: string;
    body?: string;
}
interface ConfirmOptions {
    title?: string;
    body?: string;
}
interface OpenFileOptions {
    title?: string;
    defaultPath?: string;
    filters?: Filter[];
    multiple?: boolean;
}
interface OpenFolderOptions {
    title?: string;
    defaultPath?: string;
    multiple?: boolean;
}
interface SaveFileOptions {
    title?: string;
    defaultPath?: string;
    filters?: Filter[];
}
interface Filter {
    name?: string;
    extensions?: string[];
}

export { AskOptions, ConfirmOptions, Filter, MessageOptions, OpenFileOptions, OpenFolderOptions, SaveFileOptions, ask, confirm, message, openFile, openFolder, saveFile };
