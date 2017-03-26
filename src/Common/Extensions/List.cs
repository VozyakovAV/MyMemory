using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class ListExtension
    {
        private static Random _random = new Random();
        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            int index = _random.Next(list.Count());
            return list.ToList()[index];
        }

        public static void Randomize<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<T> GetRandomizeList<T>(this IList<T> list)
        {
            var result = list.ToList();
            Randomize(result);
            return result;
        }
    }
}
