using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Debug = UnityEngine.Debug;
using ReflectionUtility;
using AnnalsMod.Code;

namespace AnnalsMod
{
    public class AnnalsModWorldLogMessageExtensions
    {
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(WorldLogMessageExtensions), "getFormatedText")]
        //public static void getExtendFormatedText(ref WorldLogMessage pMessage, UnityEngine.UI.Text pTextField, bool pColorField, bool pColorTags, ref string __result)
        //{
        //    try
        //    {
        //        if (pMessage.kingdom == null)
        //        {
        //            return;
        //        }
        //        if (!CommonData.wlmList.ContainsKey(pMessage.kingdom))
        //        {
        //            CommonData.wlmList.Add(pMessage.kingdom, new List<string>());
        //        }
        //        var logList = CommonData.wlmList[pMessage.kingdom];
        //        //logList.Add(pMessage.kingdom);

        //        logList.Add(pMessage.date + ": " + __result);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError("AnnalsModWorldLogMessageExtensions.getExtendFormatedText" + e.Message + e.StackTrace);
        //    }
        //}

        /// <summary>
        /// 历史消息-设置消息时
        /// </summary>
        /// <param name="pMessage"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HistoryHudItem), "SetMessage")]
        public static void SetMessage(ref WorldLogMessage pMessage)
        {
            try
            {
                string text = pMessage.getFormatedText(KingdomListWindow.addText("", pMessage.color_special1), pColorField: false, pColorTags: true);

                if (pMessage.kingdom != null)
                {
                    AddAnnalsLog(pMessage.kingdom, text);
                    return;
                }
                if (pMessage.city != null)
                {
                    foreach (var kingdom in MapBox.instance.kingdoms.list_civs.ToList())
                    {
                        foreach (var city in kingdom.cities)
                        {
                            if (city.name.Equals(pMessage.city.name))
                            {
                                AddAnnalsLog(kingdom, text);
                                return;
                            }
                        }
                    }
                    return;
                }
                if (pMessage.unit != null)
                {
                    if (pMessage.unit.kingdom != null)
                    {
                        AddAnnalsLog(pMessage.unit.kingdom, text);
                        return;
                    }
                }
                if (pMessage.alliance != null)
                {
                    if (pMessage.alliance.kingdoms_list.Count > 0)
                    {
                        AddAnnalsLog(pMessage.alliance.kingdoms_list[0], text);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("AnnalsModWorldLogMessageExtensions.SetMessage" + e.Message + e.StackTrace);
            }
        }

        /// <summary>
        /// 城市-完成捕获时
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "finishCapture")]
        public static bool finishCapture(Kingdom pKingdom, City __instance)
        {
            Kingdom oldKingdom = (Kingdom)Reflection.GetField(typeof(City), __instance, "kingdom");
            string text = $"{GetKingdomColorToHex(pKingdom, pKingdom.name)}征服了{GetKingdomColorToHex(oldKingdom, oldKingdom.name)}的{GetKingdomColorToHex(pKingdom, __instance.name)}";
            AddAnnalsLog(pKingdom, text);
            AddAnnalsLog(oldKingdom, text);
            return true;
        }

        /// <summary>
        /// 外交-开始叛乱
        /// </summary>
        /// <param name="pCity"></param>
        /// <param name="pActor"></param>
        /// <param name="pPlot"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DiplomacyHelpers), "startRebellion")]
        public static bool startRebellion(City pCity, Actor pActor, Plot pPlot)
        {

            CommonData.startRebellionCity = pCity;
            return true;
        }

        /// <summary>
        /// 外交-开始叛乱
        /// </summary>
        /// <param name="pCity"></param>
        /// <param name="pActor"></param>
        /// <param name="pPlot"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DiplomacyHelpers), "startRebellion")]
        public static void startRebellionPostfix(City pCity, Actor pActor, Plot pPlot)
        {
            if (CommonData.startRebellionKingdom == null)
            {
                return;
            }
            Kingdom pKingdom = (Kingdom)Reflection.GetField(typeof(City), pCity, "kingdom");
            //叛乱国家城市列表
            string citiesData = $"{GetKingdomColorToHex(pKingdom, JsonConvert.SerializeObject(pKingdom.cities.Select(o => o.name).ToList()))}";
            string text = $"{GetKingdomColorToHex(CommonData.startRebellionKingdom, CommonData.startRebellionKingdom.name)}的{citiesData}发起叛乱,建立了{GetKingdomColorToHex(pKingdom, pKingdom.name)}";
            AddAnnalsLog(pKingdom, text);
            AddAnnalsLog(CommonData.startRebellionKingdom, text);
            CommonData.startRebellionKingdom = null;
        }

        /// <summary>
        /// 城市-设置王国
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "setKingdom")]
        public static bool setKingdom(Kingdom pKingdom, City __instance)
        {
            if (CommonData.startRebellionCity != null && CommonData.startRebellionCity.Equals(__instance))
            {
                CommonData.startRebellionKingdom = (Kingdom)Reflection.GetField(typeof(City), __instance, "kingdom");
                CommonData.startRebellionCity = null;
            }
            return true;
        }

        /// <summary>
        /// 家族-检查成员
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Clan), "checkMembers")]
        public static bool checkMembers(Clan __instance)
        {
            var chief = __instance.getChief();
            if (chief == null || !chief.isAlive())
            {
                return true;
            }
            if (!CommonData.AnnalsActorInfoDict.ContainsKey(chief.base_data.id))
            {
                var info = new AnnalsActorInfo(chief.base_data.id, chief.getName(), chief.base_data.created_time.ToString(), "");
                CommonData.AnnalsActorInfoDict.Add(chief.base_data.id, info);
            }
            if (!CommonData.AnnalsClanInfoDict.ContainsKey(__instance.data.id))
            {
                var info = new AnnalsClanInfo(__instance.data.id, __instance.name, __instance.data.created_time.ToString(), chief.base_data.id, "", "");
                CommonData.AnnalsClanInfoDict.Add(__instance.data.id, info);
            }
            else
            {
                if (chief.base_data.id.Equals(CommonData.AnnalsClanInfoDict[__instance.data.id].chiefID))
                {
                    return true;
                }
                CommonData.AnnalsClanInfoDict[__instance.data.id].chiefID = chief.base_data.id;
            }
            string chiefName = GetKingdomColorToHex(__instance, chief.getName());
            string msg = $"成员{chiefName}成为家族的首领!";

            AddAnnalsLog(__instance, msg);
            return true;
        }

        /// <summary>
        /// 日志-国王死亡
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorldLog), "logKingDead")]
        public static bool logKingDead(Kingdom pKingdom, Actor pActor)
        {
            try
            {
                if (CommonData.AnnalsActorInfoDict.TryGetValue(pActor.base_data.id, out AnnalsActorInfo info))
                {
                    info.endTime = $"{World.world.mapStats.year + 1}年{World.world.mapStats.getCurrentMonth() + 1}月";
                }
                string msg = $"成员{GetKingdomColorToHex(pKingdom, pActor.getName())}魂归故里, {GetKingdomColorToHex(pKingdom, pKingdom.name)}在位年.";
                AddAnnalsLog(pActor.getClan(), msg);
            }
            catch (Exception e)
            {
                Debug.LogError("logKingDead" + e.Message + e.StackTrace);
            }
            return true;
        }

        /// <summary>
        /// 日志-战争结束
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorldLog), "logWarEnded")]
        public static bool logWarEnded(War pWar)
        {
            try
            {
                var attacker = pWar.main_attacker;
                Kingdom defender = pWar.main_defender;
                string text = $"{GetKingdomColorToHex(attacker, attacker.name)}结束了对";

                if (pWar.main_defender != null && pWar.main_defender.isAlive())
                {
                    text += $"{GetKingdomColorToHex(defender, defender.name)}";
                }
                text += $"发起的{GetKingdomColorToHex(attacker, pWar.name)}";

                //Debug.LogError(text);

                AddAnnalsLog(attacker, text);
                AddAnnalsLog(defender, text);
            }
            catch (Exception e)
            {
                Debug.LogError("logWarEnded" + e.Message + e.StackTrace);
            }

            return true;
        }


        /// <summary>
        /// 日志-王国覆灭
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorldLog), "logKingdomDestroyed")]
        public static bool logKingdomDestroyed(Kingdom pKingdom)
        {
            string text = $"{GetKingdomColorToHex(pKingdom, pKingdom.name)}覆灭了";
            AddAnnalsLog(pKingdom, text);
            if (CommonData.AnnalsKingdomInfoDict.TryGetValue(pKingdom.id, out AnnalsKingdomInfo info))
            {
                info.endTime = $"{World.world.mapStats.year + 1}年{World.world.mapStats.getCurrentMonth() + 1}月";
            }
            return true;
        }

        /// <summary>
        /// 日志-新的国王
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorldLog), "logNewKing")]
        public static bool logNewKing(Kingdom pKingdom)
        {
            string msg = $"成员{GetKingdomColorToHex(pKingdom, pKingdom.king.getName())}成为{GetKingdomColorToHex(pKingdom, pKingdom.name)}的国王!";
            AddAnnalsLog(pKingdom.king.getClan(), msg);
            return true;
        }

        /// <summary>
        /// 日志-新的战争
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorldLog), "logNewWar")]
        public static bool logNewWar(Kingdom pKingdom1, Kingdom pKingdom2)
        {
            CommonData.AnnalsKingdomInfoDict[pKingdom1.id].AddContactKingdom(pKingdom2.id);
            CommonData.AnnalsKingdomInfoDict[pKingdom2.id].AddContactKingdom(pKingdom1.id);
            return true;
        }

        /// <summary>
        /// 日志-新的联盟
        /// </summary>
        /// <param name="pKingdom"></param>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorldLog), "logAllianceCreated")]
        public static bool logAllianceCreated(Alliance pAlliance)
        {
            foreach (var k1 in pAlliance.kingdoms_list)
            {
                foreach (var k2 in pAlliance.kingdoms_list)
                {
                    CommonData.AnnalsKingdomInfoDict[k1.id].AddContactKingdom(k2.id);
                }
            }
            return true;
        }

        /// <summary>
        /// 新增编年史日记
        /// </summary>
        /// <param name="kingdom"></param>
        /// <param name="text"></param>
        public static void AddAnnalsLog(Kingdom kingdom, string text)
        {
            JudgeAddKingdom(kingdom);
            if (!CommonData.AnnalsKingdomLogsDict.ContainsKey(kingdom.id))
            {
                CommonData.AnnalsKingdomLogsDict.Add(kingdom.id, new List<AnnalsLogInfo>());
            }
            AnnalsLogInfo annalsInfo = new AnnalsLogInfo();

            annalsInfo.year = World.world.mapStats.year + 1;
            annalsInfo.month = World.world.mapStats.getCurrentMonth() + 1;
            annalsInfo.log = text;
            annalsInfo.AddIDByType(kingdom.id, AnnalsObjectType.Kingdom);
            if (kingdom.king != null && kingdom.king.getClan() != null)
            {
                Clan clan = kingdom.king.getClan();
                //if (!CommonData.ClanLogDict.ContainsKey(clan))
                //{
                //    Debug.LogWarning("新增家族" + kingdom.king.getClan().name);
                //    CommonData.ClanLogDict.Add(clan, new List<AnnalsInfo>());
                //}
                //Debug.LogError(kingdom.king.name);
                annalsInfo.AddIDByType(clan.data.id, AnnalsObjectType.Clan);

            }
            CommonData.AnnalsKingdomLogsDict[kingdom.id].Add(annalsInfo);
        }

        public static void AddAnnalsLog(Clan clan, string text)
        {
            if (clan == null)
            {
                return;
            }
            if (!CommonData.AnnalsClanLogsDict.ContainsKey(clan.data.id))
            {
                CommonData.AnnalsClanLogsDict.Add(clan.data.id, new List<AnnalsLogInfo>());
            }
            AnnalsLogInfo annalsInfo = new AnnalsLogInfo();

            annalsInfo.year = World.world.mapStats.year + 1;
            annalsInfo.month = World.world.mapStats.getCurrentMonth() + 1;
            annalsInfo.log = text;
            annalsInfo.AddIDByType(clan.data.id, AnnalsObjectType.Clan);
            CommonData.AnnalsClanLogsDict[clan.data.id].Add(annalsInfo);
        }


        /// <summary>
        /// 获取带颜色的王国文本记录
        /// </summary>
        /// <param name="kingdom"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetKingdomColorToHex(Kingdom kingdom, string text)
        {
            string color = Toolbox.colorToHex((Color32)kingdom.getColor().getColorText());
            return $"<color={color}>{text}</color>";
        }

        /// <summary>
        /// 获取带颜色的王国文本记录
        /// </summary>
        /// <param name="clan"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetKingdomColorToHex(Clan clan, string text)
        {
            string color = Toolbox.colorToHex((Color32)clan.getColor().getColorText());
            return $"<color={color}>{text}</color>";
        }

        public static void JudgeAddKingdom(Kingdom kingdom)
        {
            if (kingdom != null && kingdom.isAlive() && !CommonData.AnnalsKingdomInfoDict.ContainsKey(kingdom.id))
            {
                //var info = new AnnalsKingdomInfo(kingdom.id, kingdom.name, kingdom.data.created_time.ToString(), kingdom.king.getClan().data.id, kingdom.king.base_data.id, "", kingdom.getMotto());
                var info = new AnnalsKingdomInfo(kingdom.id, kingdom.name);
                info.createTime = kingdom.data.created_time.ToString();
                CommonData.AnnalsKingdomInfoDict.Add(info.id, info);
            }
        }
    }
}
