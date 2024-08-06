using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnalsMod.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class AnnalsActorInfo
    {
        public string id;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 出生日期
        /// </summary>
        public string createTime;
        /// <summary>
        /// 
        /// </summary>
        public string endTime;
        /// <summary>
        /// 文化标识
        /// </summary>
        public string cultureID;

        /// <summary>
        /// 历史职位集合
        /// </summary>
        public List<string> annalsPositionList;

        /// <summary>
        /// 历史密谋集合
        /// </summary>
        public List<string> annalsPlotList;

        public AnnalsActorInfo(string id, string name, string createTime, string cultureID)
        {
            this.id = id;
            this.name = name;
            this.createTime = createTime;
            this.cultureID = cultureID;
        }
        public void SetEndTime()
        {
            endTime = $"{World.world.mapStats.year + 1}年{World.world.mapStats.getCurrentMonth() + 1}月";
        }
    }
}
