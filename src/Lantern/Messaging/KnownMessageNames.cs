namespace Lantern.Messaging;

public static class KnownMessageNames
{
    public const string UpdaterOnChecked = "lantern.updater.onChecked";
    public const string UpdaterOnPrepared = "lantern.updater.onPrepared";

    public const string TrayOnMenuItemClick = "lantern.tray.onMenuItemClick";
    public const string TrayOnClick = "lantern.tray.onClick";

    public const string WindowOnClosing = "lantern.window.onClosing";
    public const string WindowOnClosed = "lantern.window.onClosed";
    public const string WindowOnResized = "lantern.window.onResized";
    public const string WindowOnMoved = "lantern.window.onMoved";
    public const string WindowOnCreated = "lantern.window.onCreated";

    public const string WindowOnMaximized = "lantern.window.onMaximized";
    public const string WindowOnMinimized = "lantern.window.onMinimized";
    public const string WindowOnFullScreen = "lantern.window.onFullScreen";
    public const string WindowOnRestore = "lantern.window.onRestore";

    public const string WindowSetAlwaysOnTop = "lantern.window.setAlwaysOnTop";
    public const string WindowSetSize = "lantern.window.setSize";
    public const string WindowSetMinSize = "lantern.window.setMinSize";
    public const string WindowSetMaxSize = "lantern.window.setMaxSize";
    public const string WindowSetPosition = "lantern.window.setPosition";
    public const string WindowSetTitle = "lantern.window.setTitle";
    public const string WindowSetUrl = "lantern.window.setUrl";
    public const string WindowCenter = "lantern.window.center";
    public const string WindowActivate = "lantern.window.activate";
    public const string WindowShow = "lantern.window.show";
    public const string WindowHide = "lantern.window.hide";
    public const string WindowClose = "lantern.window.close";

    public const string WindowMaximize = "lantern.window.maximize";
    public const string WindowMinimize = "lantern.window.minimize";
    public const string WindowRestore = "lantern.window.restore";
    public const string WindowFullScreen = "lantern.window.fullScreen";

    public const string WindowIsMaximized = "lantern.window.isMaximized";
    public const string WindowIsMinimized = "lantern.window.isMinimized";
    public const string WindowIsFullScreen = "lantern.window.isFullScreen";
    public const string WindowIsAlwaysOnTop = "lantern.window.isAlwaysOnTop";
    public const string WindowIsVisible = "lantern.window.isVisible";
    public const string WindowIsActive = "lantern.window.isActive";

    public const string WindowGetSize = "lantern.window.getSize";
    public const string WindowGetPosition = "lantern.window.getPosition";
    public const string WindowGetTitle = "lantern.window.getTitle";
    public const string WindowGetUrl = "lantern.window.getUrl";
    public const string WindowGetMinSize = "lantern.window.getMinSize";
    public const string WindowGetMaxSize = "lantern.window.getMaxSize";

    public const string WindowIsSkipTaskbar = "lantern.window.isSkipTaskbar";
    public const string WindowSetSkipTaskbar = "lantern.window.setSkipTaskbar";
    public const string WindowIsResizable = "lantern.window.isResizable";
    public const string WindowSetResizable = "lantern.window.setResizable";
    public const string WindowGetTitleBarStyle = "lantern.window.getTitleBarStyle";
    public const string WindowSetTitleBarStyle = "lantern.window.setTitleBarStyle";

    public const string WindowStartDragMove = "lantern.window.startDragMove";

    public const string WindowCreate = "lantern.window.create";
    public const string WindowGetAll = "lantern.window.getAll";

    public const string AppGetVersion = "lantern.app.getVersion";
    public const string AppShutdown = "lantern.app.shutdown";

    public const string ShellOpen = "lantern.shell.open";

    public const string ClipboardSetText = "lantern.clipboard.setText";
    public const string ClipboardGetText = "lantern.clipboard.getText";

    public const string DialogMessage = "lantern.dialog.message";
    public const string DialogAsk = "lantern.dialog.ask";
    public const string DialogConfirm = "lantern.dialog.confirm";

    public const string DialogOpenFolder = "lantern.dialog.openFolder";
    public const string DialogOpenFile = "lantern.dialog.openFile";
    public const string DialogSaveFile = "lantern.dialog.saveFile";

    public const string NotificationSend = "lantern.notification.send";

    public const string UpdaterLaunch = "lantern.updater.launch";
    public const string UpdaterCheck = "lantern.updater.check";
    public const string UpdaterPerform = "lantern.updater.perform";

    public const string EventListen = "lantern.event.listen";
    public const string EventUnlisten = "lantern.event.unlisten";

    public const string ScreenGetAll = "lantern.screen.getAll";
    public const string ScreenGetCurrent = "lantern.screen.getCurrent";

    public const string TraySetTooltip = "lantern.tray.setTooltip";
    public const string TraySetMenu = "lantern.tray.setMenu";
    public const string TrayShow = "lantern.tray.show";
    public const string TrayHide = "lantern.tray.hide";
    public const string TrayIsVisible = "lantern.tray.isVisible";

}