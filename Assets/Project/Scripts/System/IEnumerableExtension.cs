using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboQuest
{
    public static class IEnumerableExtension
    {
        public static int FirstIndex<T>(this IEnumerable<T> enumerable, Predicate<T> match)
        {
            var enumerator = enumerable.GetEnumerator();
            var index = 0;
            while (enumerator.MoveNext())
            {
                if (match(enumerator.Current))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}