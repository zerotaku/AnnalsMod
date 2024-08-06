using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnalsMod.Code
{
    public class AnnalsKingdomInfo
    {
        public string id;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;

        public string createTime;
        /// <summary>
        /// 家族标识
        /// </summary>
        public string clanID;
        /// <summary>
        /// 国王标识
        /// </summary>
        public string kingID;
        /// <summary>
        /// 文化标识
        /// </summary>
        public string cultureID;

        /// <summary>
        /// 座右铭
        /// </summary>
        public string motto;
        public string endTime;

        /// <summary>
        /// 历代国王标识集合
        /// </summary>
        public List<string> annalsKingIDList;
        /// <summary>
        /// 接触王国集合
        /// </summary>
        public HashSet<string> contactKingdomIDList;

        public AnnalsKingdomInfo(string id, string name)
        {
            this.id = id;
            this.name = name;
            annalsKingIDList = new List<string>();
            contactKingdomIDList = new HashSet<string>();
        }
        public AnnalsKingdomInfo(string id, string name, string createTime, string clanID, string kingID, string cultureID, string motto)
        {
            this.id = id;
            this.name = name;
            this.createTime = createTime;
            this.clanID = clanID;
            this.kingID = kingID;
            this.cultureID = cultureID;
            this.motto = motto;
            annalsKingIDList = new List<string>();
            contactKingdomIDList = new HashSet<string>();
        }

        public void SetEndTime()
        {
            endTime = $"{World.world.mapStats.year + 1}年{World.world.mapStats.getCurrentMonth() + 1}月";
        }

        public void AddContactKingdom(string id)
        {
            contactKingdomIDList.Add(id);
        }
    }
}
