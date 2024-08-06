using NCMS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;

namespace AnnalsMod
{
    class Buttons
    {
        public static void init()
        {
            //创建'王国列表'按钮
            var WealthKingdom = NCMS.Utils.PowerButtons.CreateButton(
            "KingdomListWindowButton",
            Resources.Load<Sprite>("ui/icons/KingdomWealth"),
            "王国列表",
            "编年史王国列表",
            Vector2.zero,
            NCMS.Utils.ButtonType.Click,
            null,
            ShowKingdomListWindow);

            //新增按钮至王国表格
            NCMS.Utils.PowerButtons.AddButtonToTab(
            WealthKingdom,
            NCMS.Utils.PowerTab.Kingdoms,
            new Vector2(795, -18));


            var WealthClan = NCMS.Utils.PowerButtons.CreateButton(
            "ClanListWindowButton",
            Resources.Load<Sprite>("ui/icons/KingdomWealth"),
            "家族列表",
            "编年史家族列表",
            Vector2.zero,
            NCMS.Utils.ButtonType.Click,
            null,
            ShowClanListWindow);

            NCMS.Utils.PowerButtons.AddButtonToTab(
            WealthClan,
            NCMS.Utils.PowerTab.Kingdoms,
            new Vector2(830, -18));

        }
        public static void ShowKingdomListWindow()
        {
            KingdomListWindow.initWindow();
            Windows.ShowWindow("KingdomListWindow");
        }

        public static void ShowClanListWindow()
        {
            ClanListWindow.initWindow();
            Windows.ShowWindow("ClanListWindow");
        }
    }
}
