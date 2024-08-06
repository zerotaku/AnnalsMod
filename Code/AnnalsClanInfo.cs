using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnalsMod.Code
{
    /// <summary>
    /// 家族信息
    /// </summary>
    public class AnnalsClanInfo
    {
        public string id;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 
        /// </summary>
        public string createTime;
        /// <summary>
        /// 
        /// </summary>
        public string endTime;
        /// <summary>
        /// 国王标识
        /// </summary>
        public string chiefID;
        /// <summary>
        /// 文化标识
        /// </summary>
        public string cultureID;

        /// <summary>
        /// 座右铭
        /// </summary>
        public string motto;
        /// <summary>
        /// 历代组长标识集合
        /// </summary>
        public List<string> annalsChiefIDList;
        /// <summary>
        /// 历代国王标识集合
        /// </summary>
        public List<string> annalsKingIDList;

        public AnnalsClanInfo(string id, string name, string createTime, string chiefID, string cultureID, string motto)
        {
            this.id = id;
            this.name = name;
            this.createTime = createTime;
            this.chiefID = chiefID;
            this.cultureID = cultureID;
            this.motto = motto;
            annalsChiefIDList= new List<string>();
            annalsKingIDList = new List<string>();
        }
        public void SetEndTime()
        {
            endTime = $"{World.world.mapStats.year + 1}年{World.world.mapStats.getCurrentMonth() + 1}月";
        }
    }
}
