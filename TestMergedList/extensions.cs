using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMergedList
{
    public static class extensions
    {
        public static List<T> MergeWith<T, TKey>(this List<T> list, List<T> other, Func<T, TKey> keySelector, Func<T, T, T> merge)
        {
            var newList = new List<T>();
            foreach (var item in list)
            {
                var otherItem = other.SingleOrDefault((i) => keySelector(i).Equals(keySelector(item)));
                if (otherItem != null)
                {
                    newList.Add(merge(item, otherItem));
                }
            }
            return newList;
        }
    }
}
