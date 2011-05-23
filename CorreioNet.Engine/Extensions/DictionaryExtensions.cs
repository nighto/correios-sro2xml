using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorreioNet.Engine.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Copy all items from another Dictionary to this.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="source"></param>
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> source) 
        {
            foreach (var item in source)
            {
                dictionary.Add(item.Key, item.Value);
            }
        }
    }
}
