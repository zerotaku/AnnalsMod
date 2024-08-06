using System.Collections.Generic;
using AnnalsMod.Code;
using UnityEngine;
using UnityEngine.UI;

namespace AnnalsMod
{
    class CommonData
    {


        public static Dictionary<string, List<AnnalsLogInfo>> AnnalsKingdomLogsDict = new Dictionary<string, List<AnnalsLogInfo>>();
        public static Dictionary<string, List<AnnalsLogInfo>> AnnalsClanLogsDict = new Dictionary<string, List<AnnalsLogInfo>>();
        public static Dictionary<string, List<AnnalsLogInfo>> AnnalsWarLogsDict = new Dictionary<string, List<AnnalsLogInfo>>();
        public static Dictionary<string, List<AnnalsLogInfo>> AnnalsActorLogsDict = new Dictionary<string, List<AnnalsLogInfo>>();
        //public static Dictionary<string, List<AnnalsLogInfo>> AnnalsActorLogsDict = new Dictionary<string, List<AnnalsLogInfo>>();
        
        public static Dictionary<string, AnnalsKingdomInfo> AnnalsKingdomInfoDict = new Dictionary<string, AnnalsKingdomInfo>();
        public static Dictionary<string, AnnalsClanInfo> AnnalsClanInfoDict = new Dictionary<string, AnnalsClanInfo>();
        public static Dictionary<string, AnnalsActorInfo> AnnalsActorInfoDict = new Dictionary<string, AnnalsActorInfo>();

        public static string selectAnnalsKingdom;
        public static string selectAnnalsClan;

        // public static List<string> kingdomList = new List<string>();
        //public static List<string> WorldLogList = new List<string>();
        public static City startRebellionCity = null;
        public static Kingdom startRebellionKingdom = null;

        public static void init()
        {

        }

        public static string getFormatedText(WorldLogMessage pMessage)
        {
            Text text = KingdomListWindow.addText("", new Color(0.9f, 0.6f, 0, 1));
            var data = pMessage.getFormatedText(text, false, false);
            return data;
        }



        public static GameObject addBanner(GameObject parent, GameObject content, Kingdom kingdom)
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
            BannerLoader bannerLoader = BannerGO.AddComponent<BannerLoader>();
            bannerLoader.partIcon = iconImage;
            bannerLoader.partBackround = backgroundImage;
            bannerLoader.load(kingdom);
            return BannerGO;
        }

        public static GameObject addBanner(GameObject parent, GameObject content, Clan clan)
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



        public static GameObject addBanner(GameObject parent, GameObject content, Culture clan)
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
            BannerLoaderCulture bannerLoader = BannerGO.AddComponent<BannerLoaderCulture>();
            bannerLoader.partIcon = iconImage;
            bannerLoader.load(clan);
            return BannerGO;
        }


    }




}
