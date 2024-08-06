using System.Collections.Generic;
using AnnalsMod.UI.Windows;
using NeoModLoader.General;
using NeoModLoader.General.UI.Tab;

namespace AnnalsMod.UI;

internal static class AnnalsTab
{
    public const string INFO = "info";
    public const string DISPLAY = "display";
    public const string CREATURE = "creature";
    public static PowersTab tab;
    public static AnnalsAutoLayoutWindow annalsAutoLayoutWindow;

    public static void Init()
    {
        // Create a tab with id "Example", title key "tab_example", description key "hotkey_tip_tab_other", and icon "ui/icons/iconSteam".
        // 创建一个id为"Example", 标题key为"tab_example", 描述key为"hotkey_tip_tab_other", 图标为"ui/icons/iconSteam"的标签页.
        tab = TabManager.CreateTab("Annals", "编年史", "编年史标签",
            SpriteTextureLoader.getSprite("ui/icons/iconSteam"));
        // Set the layout of the tab. The layout is a list of strings, each string is a category. Names of each category are not important.
        // 设置标签页的布局. 布局是一个字符串列表, 每个字符串是一个分类. 每个分类的名字不重要.
        tab.SetLayout(new List<string>()
        {
            INFO,
            DISPLAY,
            CREATURE
        });
        // Create windows.
        // 创建窗口.
        _createWindows();
        // Add buttons to the tab.
        // 向标签页添加按钮.
        _addButtons();
        // Update the layout of the tab.
        // 更新标签页的布局.
        tab.UpdateLayout();
    }

    private static void _createWindows()
    {
        annalsAutoLayoutWindow = AnnalsAutoLayoutWindow.CreateWindow(nameof(AnnalsAutoLayoutWindow),
            "王国集合");
    }

    private static void _addButtons()
    {
        tab.AddPowerButton(INFO,
            PowerButtonCreator.CreateWindowButton("test_1", nameof(AnnalsAutoLayoutWindow),
                SpriteTextureLoader.getSprite("ui/icons/iconKingdom")));
    }

    public static void Reload()
    {
        //annalsAutoLayoutWindow.InitList();
    }
}