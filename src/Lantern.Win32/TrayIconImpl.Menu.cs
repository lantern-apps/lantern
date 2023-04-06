using Lantern.Windows;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public partial class TrayIconImpl
{
    public void ShowContextMenu()
    {
        if (Menu?.Items == null || Menu.Items.Count == 0)
            return;
        GetCursorPos(out POINT pt);
        SetForegroundWindow(Win32Platform.Instance.Handle);

        var screen = Win32Platform.Instance.Screen.ScreenFromPoint(new PhysicsPosition(pt.X, pt.Y));
        if (screen != null)
        {
            ShowContextMenu(pt.X, screen.PhysicsWorkingArea.Height);
        }
        else
        {
            ShowContextMenu(pt.X, pt.Y);
        }
    }

    private void ShowContextMenu(int x, int y)
    {
        var items = Menu!.Items;
        var flags =
           TRACK_POPUP_MENU_FLAGS.TPM_RETURNCMD |
           TRACK_POPUP_MENU_FLAGS.TPM_NONOTIFY |
           TRACK_POPUP_MENU_FLAGS.TPM_BOTTOMALIGN;

        int id;

        nuint lastId = 1;
        List<DestroyMenuSafeHandle> safeHandles = new();
        Dictionary<nuint, MenuItem> ids = new();

        void AppendToMenu(nint menu, ICollection<MenuItem> items)
        {
            var handle = new DestroyMenuSafeHandle(menu, true);
            safeHandles.Add(handle);

            MENU_ITEM_FLAGS GetItemFlags(MenuItem item)
            {
                var flags = (MENU_ITEM_FLAGS)0;

                if (!item.Enabled)
                {
                    flags |= MENU_ITEM_FLAGS.MF_DISABLED;
                    flags |= MENU_ITEM_FLAGS.MF_GRAYED;
                }

                if (item.Checked)
                {
                    flags |= MENU_ITEM_FLAGS.MF_CHECKED;
                }

                return flags;
            }

            foreach (var item in items)
            {
                if (item.Visible == false)
                    continue;

                nuint id = lastId++;
                ids.Add(id, item);

                if (item.Type == MenuItemType.Separator)
                {
                    AppendMenu(handle, MENU_ITEM_FLAGS.MF_SEPARATOR, id, null!);
                }
                else if (item.Type == MenuItemType.Normal)
                {
                    AppendMenu(handle, GetItemFlags(item), id, item.Text ?? string.Empty);
                }
                else if (item.Type == MenuItemType.SubMenu)
                {
                    if (item.Items == null || item.Items.Length == 0)
                        continue;

                    var subMenuHandle = CreatePopupMenu();
                    AppendToMenu(subMenuHandle, item.Items);
                    AppendMenu(
                        hMenu: handle,
                        uFlags: MENU_ITEM_FLAGS.MF_POPUP,
                        uIDNewItem: (nuint)subMenuHandle,
                        lpNewItem: item.Text ?? string.Empty);
                }
            }
        }

        unsafe
        {
            var _hMenu = CreatePopupMenu();
            AppendToMenu(_hMenu, items!);

            id = TrackPopupMenuEx(
                hMenu: _hMenu,
                uFlags: (uint)flags,
                x: x,
                y: y,
                hwnd: Win32Platform.Instance.Handle,
                lptpm: null);

            var exceptions = new List<Exception>();
            foreach (var handle in safeHandles)
            {
                try
                {
                    handle.Dispose();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
            if (exceptions.Any()) throw new AggregateException(exceptions);
        }

        if (id == 0)
            return;

        if (ids.TryGetValue((nuint)id, out var item))
            Menu.ItemClick?.Invoke(item);
    }

    //private static NativeMenuItemBase? SearchForId(IEnumerable<NativeMenuItemBase> items, int itemId)
    //{
    //    foreach (var item in items)
    //    {
    //        if (item is NativeMenuSubMenu subMenu)
    //        {
    //            var foundItem = SearchForId(subMenu.Items, itemId);
    //            if (foundItem is not null)
    //                return foundItem;
    //        }

    //        if (item is not NativeMenuItem menuItem)
    //            continue;

    //        if (menuItem.Id == itemId)
    //            return item;
    //    }

    //    return null;
    //}

}
