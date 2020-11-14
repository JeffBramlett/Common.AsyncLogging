using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AsyncLogging
{
    public static class Extensions
    {
        public static IList<KeyValuePair<string, object>> ConvertToKeyValuePairs(this Dictionary<string, object> dictionary)
        {
            IList<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();

            foreach(var key in dictionary.Keys)
            {
                list.Add(new KeyValuePair<string, object>(key, dictionary[key]));
            }

            return list;
        }
    }
}
