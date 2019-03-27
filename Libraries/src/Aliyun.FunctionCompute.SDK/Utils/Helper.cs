using System;
using System.Collections.Generic;

namespace Aliyun.FunctionCompute.SDK.Utils
{
    public static class Helper
    {

        public static Dictionary<string, string> MergeDictionary(Dictionary<string, string> first, Dictionary<string, string> second)
        {
            if (first == null) first = new Dictionary<string, string>();
            if (second == null) return first;

            foreach (string key in second.Keys)
            {
                if (!first.ContainsKey(key))
                    first.Add(key, second[key]);
            }
            return first;
        }
    }
}
