using System;
using System.Linq;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace AnnalsMod
{
    class ClanListWindow : MonoBehaviour
    {
        public static int MoveDown = 0;
        public static GameObject scrollView;
        public static GameObject content;
        public static ScrollWindow window;
        public static void init()
        {
            window = Windows.CreateNewWindow("ClanListWindow", "家族史列表");
            scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View");
            scrollView.gameObject.SetActive(true);
            content = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View/Viewport/Content");
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100000);
        }
        public static void initWindow()
        {
            foreach (Transform child in content.transform)
            {
                GameObject.Destroy(child.gameObject);
                MoveDown = 0;
            }
            window = Windows.CreateNewWindow("ClanListWindow", "家族史列表");
            AddData();
        }
        public static void AddData()
        {
            try
            {
                //总数
                Text KingdomText = addText(World.world.clans.list.Count().ToString() + "/" + World.world.kingdoms.list_civs.Count().ToString(), new Vector3(210, -55, 0), new Color(0.4f, 0.4f, 0.4f, 0.5f));
                KingdomText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                //var FirstList = CommonData.kingdomList.ToList();
                //FirstList.Sort((pair1, pair2) => pair2.Value.Values.Count.CompareTo(pair1.Value.Values.Count));
                //var FinalList = FirstList.Select(x => x.Key).ToList();
                //当前王国列表
                var clanList = MapBox.instance.clans.list.ToList();

                foreach (var clan in clanList)
                {

                    MoveDown -= 40;

                    GameObject BgHolderGO = new GameObject("BgHolderGO");
                    BgHolderGO.transform.SetParent(content.transform);
                    RectTransform BgHolderRect = BgHolderGO.AddComponent<RectTransform>();
                    BgHolderRect.localPosition = new Vector3(130, MoveDown, 0);
                    BgHolderRect.sizeDelta = new Vector2(600, 100);


                    GameObject TextBgHolderGO = new GameObject("TextBgHolderGO");
                    TextBgHolderGO.transform.SetParent(content.transform);
                    RectTransform TextBgHolderRect = TextBgHolderGO.AddComponent<RectTransform>();
                    TextBgHolderRect.localPosition = new Vector3(160, MoveDown, 0);
                    TextBgHolderRect.sizeDelta = new Vector2(200, 60);


                    GameObject BannerHolderGO = new GameObject("BannerHolderGO");
                    BannerHolderGO.transform.SetParent(content.transform);
                    RectTransform BannerHolderRect = BannerHolderGO.AddComponent<RectTransform>();
                    BannerHolderRect.localPosition = new Vector3(50, MoveDown, 0);
                    BannerHolderRect.sizeDelta = new Vector2(100, 100);

                    GameObject banner = CommonData.addBanner(BannerHolderGO,content, clan);
                    RectTransform bannerRect = banner.AddComponent<RectTransform>();
                    bannerRect.localPosition = new Vector3(100, MoveDown, 0);
                    bannerRect.sizeDelta = new Vector2(150, 150);
                     

                    Text titleText = addText(clan.name, new Vector3(150, -10 + MoveDown, 0), new Color(0.9f, 0.6f, 0, 1), clan);
                    titleText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);

                }

            }
            catch (Exception ex)
            {
                Debug.LogWarning("更新数据异常:" + ex.Message + ex.StackTrace);
            }
            
        }


        //查看王国详情
        public static void ShowClan(string clan)
        {
            CommonData.selectAnnalsClan = clan;
            ClanInfoWindow.initWindow();
            Windows.ShowWindow("ClanInfoWindow");
        }


        public static Text addText(string textString, Vector3 pos, Color Color, Clan clan = null)
        {
            GameObject name = window.transform.Find("Background").Find("Name").gameObject;
            GameObject textGo = Instantiate(name, content.transform);
            textGo.SetActive(true);
            var textComp = textGo.GetComponent<Text>();
            textComp.text = textString;
            textComp.color = Color;
            textComp.fontSize = 10;
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.localPosition = pos;
            textRect.sizeDelta = new Vector2(100, 100);
            textGo.AddComponent<GraphicRaycaster>();
            if (clan != null)
            {
                Button buttonI = textGo.AddComponent<Button>();
                buttonI.onClick.AddListener(() => ShowClan(clan.data.id));
            }
            return textComp;
        }

        public static Text addText(string textString, Color Color)
        {
            GameObject name = window.transform.Find("Background").Find("Name").gameObject;
            GameObject textGo = Instantiate(name, content.transform);
            textGo.SetActive(true);
            var textComp = textGo.GetComponent<Text>();
            textComp.text = textString;
            textComp.color = Color;
            textComp.fontSize = 10;
            textGo.AddComponent<GraphicRaycaster>();
            return textComp;
        }
    }
}
