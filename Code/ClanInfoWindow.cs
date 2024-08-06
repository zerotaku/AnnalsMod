using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using ReflectionUtility;
using Newtonsoft.Json;
using AnnalsMod.Code;

namespace AnnalsMod
{
    class ClanInfoWindow : MonoBehaviour
    {
        public static GameObject scrollView;
        public static GameObject content;
        public static ScrollWindow window;

        public static void init()
        {
            window = Windows.CreateNewWindow("ClanInfoWindow", "家族史");
            scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View");
            scrollView.gameObject.SetActive(true);
            content = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View/Viewport/Content");
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100000);
        }
        public static void initWindow()
        {
            foreach (Transform child in content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            window = Windows.CreateNewWindow("ClanInfoWindow", "家族史");
            AddData();
        }
        public static void AddData()
        {
            try
            {
                GameObject BannerHolderGO = new GameObject("BannerHolderGO");
                BannerHolderGO.transform.SetParent(content.transform);
                RectTransform BannerHolderRect = BannerHolderGO.AddComponent<RectTransform>();
                BannerHolderRect.localPosition = new Vector3(50, 30, 0);
                BannerHolderRect.sizeDelta = new Vector2(100, 100);


                var selectAnnalsClan = World.world.clans.get(CommonData.selectAnnalsClan);

                GameObject banner = addBanner(BannerHolderGO, selectAnnalsClan);
                RectTransform bannerRect = banner.AddComponent<RectTransform>();
                bannerRect.localPosition = new Vector3(150, 30, 0);
                bannerRect.sizeDelta = new Vector2(150, 150);

                Text NameText = addText(selectAnnalsClan.name, new Vector3(230, 30, 0), 10);
                NameText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                int ypoit = 0;
                int selYear = 0;
                int selMonth = 0;


                foreach (var annals in CommonData.AnnalsClanLogsDict[CommonData.selectAnnalsClan])
                {
                    //if (annals.GetClan(out Clan selClan))
                    //{
                    //    if (CommonData.AnnalsActorInfoDict.TryGetValue(selClan.getChief().base_data.id, out AnnalsActorInfo info))
                    //    {
                    //        string color = $"<color={Toolbox.colorToHex((Color32)selClan.getColor().k_color_0)}>";
                    //        //Text clanText = addText($"{color}{selClan.name}</color>", new Vector3(160, ypoit + 10, 0), new Color(0.9f, 0.6f, 0, 1), 6);
                    //        Text clanText = addText($"{info.name}", new Vector3(100, ypoit + 10, 0), 6);
                    //        clanText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 100);
                    //    }
                    //}
                    if (!selYear.Equals(annals.year))
                    {
                        Text yearText = addText($"{annals.year}年", new Vector3(175, ypoit + 10, 0), 6);
                        yearText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 100);
                        selYear = annals.year;

                        if (!selMonth.Equals(annals.month))
                        {
                            Text monthText = addText($"{annals.month}月", new Vector3(195, ypoit + 10, 0), 6);
                            monthText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 100);
                            selMonth = annals.month;
                        }
                    }

                    Text logText = addText($"{annals.log}", new Vector3(210, ypoit + 10, 0), 6);
                    logText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 100);

                    ypoit -= 5;
                    if (annals.log.Length > 20)
                    {
                        ypoit -= 5;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"打开家族界面异常:[msg:{ex.Message}][{ex.StackTrace}]");
            }
        }

        public static GameObject addBanner(GameObject parent, Clan clan)
        {
            GameObject BannerGO = new GameObject("bannerHolder");
            BannerGO.transform.SetParent(content.transform);
            BannerGO.AddComponent<CanvasRenderer>();
            GameObject backgroundGO = new GameObject("background");
            backgroundGO.transform.SetParent(BannerGO.transform);
            Image backgroundImage = backgroundGO.AddComponent<Image>();
            GameObject iconGO = new GameObject("icon");
            iconGO.transform.SetParent(BannerGO.transform);
            Image iconImage = iconGO.AddComponent<Image>();
            BannerLoaderClans bannerLoader = BannerGO.AddComponent<BannerLoaderClans>();
            bannerLoader.partIcon = iconImage;
            bannerLoader.partBackround = backgroundImage;
            bannerLoader.load(clan);
            return BannerGO;
        }

        public static Text addText(string textString, Vector3 pos, int size)
        {
            GameObject name = window.transform.Find("Background").Find("Name").gameObject;
            GameObject textGo = Instantiate(name, content.transform);
            textGo.SetActive(true);
            var textComp = textGo.GetComponent<Text>();
            textComp.text = textString;
            textComp.supportRichText = true;
            textComp.alignment = TextAnchor.LowerLeft;
            textComp.fontSize = size;
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.localPosition = pos;
            textRect.sizeDelta = new Vector2(80, 100);
            textGo.AddComponent<GraphicRaycaster>();
            return textComp;
        }
    }
}
