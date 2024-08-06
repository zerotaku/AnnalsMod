using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnalsMod.Code
{

    public class AnnalsLogInfo
    {
        public int year;

        public int month;

        public string log;

        public string disposeLog;

        private Dictionary<string, AnnalsObjectType> idKeyTypeDict;

        public AnnalsLogInfo()
        {
            idKeyTypeDict = new Dictionary<string, AnnalsObjectType>();
        }

        public void AddIDByType(string id, AnnalsObjectType type)
        {
            if (!idKeyTypeDict.ContainsKey(id))
            {
                return;
            }
            idKeyTypeDict.Add(id, type);
        }

        public bool GetIDByType(AnnalsObjectType type,out string id)
        {
            id = "";

            if (!idKeyTypeDict.ContainsValue(type))
            {
                return false;
            }
            foreach (var item in idKeyTypeDict)
            {
                if (item.Value == type)
                {
                    id = item.Key;
                    return true;
                }
            }
            return false;
        }

    }

    public enum AnnalsObjectType
    {
        Kingdom,
        Clan,
        Actor,
        War

    }
}
