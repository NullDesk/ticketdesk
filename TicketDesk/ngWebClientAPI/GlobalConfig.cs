using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ngWebClientAPI
{
    public static class GlobalConfig
    {
        static Dictionary<string, List<string>> _categories;
        static JStringList _priorities;
        static JStringList _ticketTypes;

        public static Dictionary<string, List<string>> categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = setDictionary();
            }
        }

        public static JStringList priorities
        {
            get
            {
                return _priorities;
            }
            set
            {
                _priorities = setPriorityList();
            }
        }

        public static JStringList ticketTypes
        {
            get
            {
                return _ticketTypes;
            }
            set
            {
                _ticketTypes = setTicketTypeList();
            }
        }

        private static Dictionary<string, List<string>> setDictionary()
        {
            Dictionary<string, List<string>> categoryDict = new Dictionary<string, List<string>>();
            var section = (ConfigurationManager.GetSection("CategorySettings") as System.Collections.Hashtable)
                 .Cast<System.Collections.DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());
            foreach (var item in section)
            {
                if (categoryDict.ContainsKey(item.Value))
                {
                    categoryDict[item.Value].Add(item.Key);
                }
                else
                {
                    categoryDict.Add(item.Value, new List<string>() { item.Key });
                }
            }
            return categoryDict;
        }

        private static JStringList setPriorityList()
        {
            JStringList priorityList = new JStringList();
            var data = ConfigurationManager.GetSection("PrioritySettings") as System.Collections.Specialized.NameValueCollection;
            priorityList.list = data["Priorities"].Split(',').ToList();
            return priorityList;
        }

        private static JStringList setTicketTypeList()
        {
            JStringList typeList = new JStringList();
            var data = ConfigurationManager.GetSection("TicketTypeSettings") as System.Collections.Specialized.NameValueCollection;
            typeList.list = data["TicketTypes"].Split(',').ToList();
            return typeList;
        }
    }

    public class JStringList
    {
        public List<string> list { get; set; }
    }
}