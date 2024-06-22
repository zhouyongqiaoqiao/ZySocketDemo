using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ZySocketCore.Extension
{
    internal static class CollectionExtension
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection==null || !collection.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// 随机取出一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T RandomItem<T>(this ICollection<T> collection)
        {
            if (collection.IsNullOrEmpty()) return default(T);
            int index = new Random().Next(collection.Count);
            return collection.ToArray()[index];
        }           
    }
}
