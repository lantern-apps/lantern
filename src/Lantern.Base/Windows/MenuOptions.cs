using Lantern.Messaging.Serialization;
using System.Text.Json.Serialization;

namespace Lantern.Windows;

public class MenuOptions
{
    [JsonPropertyName("items")]
    public List<MenuItemOptions> Items { get; set; } = new();

    public Menu? Build()
    {
        if (Items == null)
            return null;

        return new()
        {
            Items = BuildItems(Items)
        };

        static List<MenuItem> BuildItems(IEnumerable<MenuItemOptions>? menuItems)
        {
            if (menuItems == null)
                return new();

            var items = new List<MenuItem>();
            foreach (var item in menuItems)
            {
                MenuItem menuItem;
                switch (item.Type)
                {
                    case MenuItemType.Normal:
                        menuItem = new MenuItem(item.Id, item.Text)
                        {
                            Checked = item.Checked,
                            Enabled = item.Enabled,
                            State = item.Command
                        };
                        break;
                    case MenuItemType.Separator:
                        menuItem = MenuItem.Separator;
                        break;
                    case MenuItemType.SubMenu:
                        menuItem = new MenuItem(item.Text, BuildItems(item.Items));
                        break;
                    default:
                        continue;
                }
                items.Add(menuItem);
            }
            return items;
        }
    }
}

public class MenuItemOptions
{
    [JsonConstructor]
    public MenuItemOptions() { }

    public MenuItemOptions(string id, string text, string? command = null)
    {
        Id = id;
        Text = text;
        Type = MenuItemType.Normal;
        Command = command;
    }

    public MenuItemOptions(string text, IEnumerable<MenuItemOptions> items)
    {
        Text = text;
        Items = items.ToArray();
        Type = MenuItemType.SubMenu;
    }

    [JsonStringEnumConverter]
    [JsonPropertyName("type")]
    public MenuItemType Type { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("items")]
    public MenuItemOptions[]? Items { get; set; }

    [JsonPropertyName("checked")]
    public bool Checked { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonPropertyName("command")]
    public string? Command { get; set; }

    public static MenuItemOptions Separator { get; } = new MenuItemOptions { Type = MenuItemType.Separator };
}
