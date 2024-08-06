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

namespace AnnalsMod
{
    class KingdomInfoWindow : MonoBehaviour
    {
        public static GameObject scrollView;
        public static GameObject content;
        public static ScrollWindow window;

        public Kingdom SelKingdom;

        public static void init()
        {
            window = Windows.CreateNewWindow("KingdomInfoWindow", "王国史");
            scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View");
            scrollView.gameObject.SetActive(true);
            content = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View/Viewport/Content");
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 100000);
        }
        public static void initWindow()
        {
            foreach (Transform child in content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            window = Windows.CreateNewWindow("KingdomInfoWindow", "王国史");
            AddData();
        }
        public static void AddData()
        {
            try
            {

                GameObject BgHolderGO = new GameObject("BgHolderGO");
                BgHolderGO.transform.SetParent(content.transform);
                RectTransform BgHolderRect = BgHolderGO.AddComponent<RectTransform>();
                BgHolderRect.localPosition = new Vector3(130, 30, 0);
                BgHolderRect.sizeDelta = new Vector2(600, 100);


                GameObject TextBgHolderGO = new GameObject("TextBgHolderGO");
                TextBgHolderGO.transform.SetParent(content.transform);
                RectTransform TextBgHolderRect = TextBgHolderGO.AddComponent<RectTransform>();
                TextBgHolderRect.localPosition = new Vector3(160, 30, 0);
                TextBgHolderRect.sizeDelta = new Vector2(200, 60);


                GameObject BannerHolderGO = new GameObject("BannerHolderGO");
                BannerHolderGO.transform.SetParent(content.transform);
                RectTransform BannerHolderRect = BannerHolderGO.AddComponent<RectTransform>();
                BannerHolderRect.localPosition = new Vector3(50, 30, 0);
                BannerHolderRect.sizeDelta = new Vector2(100, 100);

                var selectKingdom = World.world.kingdoms.getKingdomByID(CommonData.selectAnnalsKingdom);

                if (selectKingdom != null && selectKingdom.isAlive())
                {
                    GameObject banner = CommonData.addBanner(BannerHolderGO, content, selectKingdom);
                    RectTransform bannerRect = banner.AddComponent<RectTransform>();
                    bannerRect.localPosition = new Vector3(200, 30, 0);
                    bannerRect.sizeDelta = new Vector2(50, 150);

                }

                //
                Text kingdomNameText = addText(selectKingdom.name + ":" + selectKingdom.id,
                    new Vector3(230, 30, 0), 10);
                kingdomNameText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                Text localTest4 = addText("200,50", new Vector3(200, 0, 0), 5);
                localTest4.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);

                Text localTest = addText("200,100", new Vector3(200, 0, 0), 5);
                localTest.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);


                Text localTest2 = addText("200,150", new Vector3(200, 0, 0), 5);
                localTest2.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);


                Text localTest3 = addText("200,200", new Vector3(200, 0, 0), 5);
                localTest3.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);


                int xpoit = 0;
                Debug.LogError(JsonConvert.SerializeObject(CommonData.AnnalsKingdomInfoDict[selectKingdom.id]));
                foreach (var kingdomID in CommonData.AnnalsKingdomInfoDict[selectKingdom.id].contactKingdomIDList)
                {
                    Text contactNameText = addText(CommonData.AnnalsKingdomInfoDict[kingdomID].name, new Vector3(170 + xpoit, 80, 0), 10);
                    contactNameText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                    xpoit += 50;
                }

                int ypoit = 0;
                int selYear = 0;
                int selMonth = 0;

                foreach (var annals in CommonData.AnnalsKingdomLogsDict[selectKingdom.id])
                {
                    Clan selClan = selectKingdom.king.getClan();
                    //if (annals.GetIDByType(out Clan selClan) && selClan.isAlive())
                    if (selClan.isAlive())
                    {
                        //GameObject BannerHolderGO = new GameObject("BannerHolderGO");
                        //BannerHolderGO.transform.SetParent(content.transform);
                        //RectTransform BannerHolderRect = BannerHolderGO.AddComponent<RectTransform>();
                        //BannerHolderRect.localPosition = new Vector3(50, MoveDown, 0);
                        //BannerHolderRect.sizeDelta = new Vector2(100, 100);

                        //GameObject bannerCaln = CommonData.addBanner(BannerHolderGO, content, selClan);
                        //RectTransform bannerRectCaln = bannerCaln.AddComponent<RectTransform>();
                        //bannerRectCaln.localPosition = new Vector3(50, ypoit + 10, 0);
                        //bannerRectCaln.sizeDelta = new Vector2(150, 150);

                        string text = $"<color={Toolbox.colorToHex((Color32)selectKingdom.getColor().k_color_0)}>{selClan.name}</color>";
                        Text clanText = addText($"{text}", new Vector3(150, ypoit + 10, 0), 6, selClan);
                        clanText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 100);
                    }

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

                //foreach (var item in HistoryHud.historyItems)
                //{
                //    string formatedText = item.textField.text;
                //    Text titleText = addText($"{formatedText}", new Vector3(140, ypoit + 10, 0), new Color(0.9f, 0.6f, 0, 1), 6);
                //    titleText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                //    //Text mesText = addText(formatedText, new Vector3(140, ypoit, 0), new Color(0.9f, 0.6f, 0, 1), 6);
                //    //mesText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                //    ypoit -= 40;
                //} 
            }
            catch (Exception e)
            {
                Debug.LogError("AddData" + e.Message + e.StackTrace);
            }
        }

        public static void ShowKingClan(string clan)
        {
            CommonData.selectAnnalsClan = clan;
            ClanInfoWindow.initWindow();
            Windows.ShowWindow("ClanInfoWindow");
        }

        public static Text addText(string textString, Vector3 pos, int size, Clan clan = null)
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
            if (clan != null)
            {
                Button buttonI = textGo.AddComponent<Button>();
                buttonI.onClick.AddListener(() => ShowKingClan(clan.data.id));
            }

            return textComp;
        }
    }
}
