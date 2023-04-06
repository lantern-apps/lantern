using System.Text.Json.Serialization;

namespace Lantern.Windows;

public class Menu
{
    public List<MenuItem> Items { get; set; } = new();

    public MenuItem? FindItem(string id)
    {
        if (Items == null)
            return null;

        return FindItem(Items, id);

        static MenuItem? FindItem(IEnumerable<MenuItem> items, string id)
        {
            foreach (var item in items)
            {
                if (item.Id == id)
                    return item;

                if (item.Items != null && item.Items.Length > 0)
                {
                    var result = FindItem(item.Items, id);
                    if (result != null) return result;
                }
            }
            return null;
        }
    }

    public Action<MenuItem>? ItemClick;
}

public class MenuItem
{
    public MenuItem() { }

    public MenuItem(string? id, string? text)
    {
        Id = id;
        Text = text;
        Type = MenuItemType.Normal;
    }

    public MenuItem(string? text, IEnumerable<MenuItem>? items)
    {
        Text = text;
        Items = items?.ToArray();
        Type = MenuItemType.SubMenu;
    }

    public MenuItemType Type { get; set; }

    public string? Id { get; set; }
    public string? Text { get; set; }
    public bool Checked { get; set; }
    public bool Enabled { get; set; } = true;
    public bool Visible { get; set; } = true;
    public object? State { get; set; }

    public MenuItem[]? Items { get; set; }

    public static MenuItem Separator { get; } = new MenuItem { Type = MenuItemType.Separator };
}

public enum MenuItemType
{
    Normal,
    SubMenu,
    Separator,
}
