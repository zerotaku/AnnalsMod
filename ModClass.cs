using AnnalsMod.UI;
using HarmonyLib;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System.IO;
using UnityEngine;

namespace AnnalsMod
{
    public class ModClass : BasicMod<ModClass>, IReloadable
    {
        internal static Transform prefab_library;

        protected override void OnModLoad()
        {
            prefab_library = new GameObject("PrefabLibrary").transform;
            prefab_library.SetParent(transform);
            // Load your mod here
            // 加载你的mod内容

            Config.isEditor = true;
            // LogInfo(GetConfig()["Default"]["WhatToSay"].TextVal); // Call this only then you confirm it is a text config item
            LogInfo(GetConfig()["Default"]["WhatToSay"].GetValue() as string);

            Init();
        }

        private void Init()
        {
            AnnalsTab.Init();
            CommonData.init();
            Buttons.init();
            KingdomListWindow.init();
            KingdomInfoWindow.init();
            ClanInfoWindow.init();
            ClanListWindow.init();
            //创建并修补所有
            Harmony.CreateAndPatchAll(typeof(AnnalsModWorldLogMessageExtensions));
            Config.setWorldSpeed(20f, false);//游戏速度调制40
        }

        [Hotfixable]
        // This method will be called when the mod is reloaded. "OnModLoad" won't be called when mod reloaded. To test it, you can modify the code in 
        // 这个函数会在模组函数热更新后被调用. "OnModLoad"仅在模组加载时被调用, 重载时不会被调用
        public void Reload()
        {
            // Reload logic here. It mainly reloads traits added and applies traits' modification to every units immediately here.
            // 重载逻辑在这里. 这里的功能主要是重新初始化特质, 并将特质的修改立即应用于所有单位.

            // Reload locales when mod reloaded, it's optional.
            // 重载模组时重新加载语言文件, 不是必需的
            var locale_dir = GetLocaleFilesDirectory(GetDeclaration());
            foreach (var file in Directory.GetFiles(locale_dir))
            {
                if (file.EndsWith(".json"))
                {
                    LM.LoadLocale(Path.GetFileNameWithoutExtension(file), file);
                }
                else if (file.EndsWith(".csv"))
                {
                    LM.LoadLocales(file);
                }
            }
            LM.ApplyLocale();
            // Reload mod resources when mod reloaded, it's optional.
            // 重载模组时重新加载模组资源, 不是必需的
            // Code is coming soon.
            // 代码很快就有了

            // Emulate methods modified.(Because code is static, it can't be modified automatically. I mean the example code is static and it cannot modify it automatically.)
            // 用于模拟函数被修改(因为代码是静态的, 不能自动修改, 我的意思是示例代码是静态的, 它不会自动修改)
            //_reload_switch = !_reload_switch;
            // Reload Example Trait. It's optional.
            // 重载示例特质, 不是必需的.
            //ExampleTraits.Init();

            // Apply traits' modification to every units immediately here.
            // 将特质的效果更新即时应用于所有单位. 
            /*foreach (var actor in World.world.units)
            {
                // Search all units and apply traits' modification to them.
                // 搜索所有拥有ExampleTrait的单位并将特质的效果更新即时应用于它们.
                if (actor != null && actor.isAlive() && actor.hasTrait("ExampleTrait"))
                {
                    actor.setStatsDirty();
                }
            }*/
        }
    }
}