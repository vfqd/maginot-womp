using System.Collections.Generic;
using Framework;

namespace Library.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list.ChooseRandom();
        }
        
        public static List<T> GetXRandomElements<T>(this List<T> list, int x)
        {
            var l = new List<T>(list);
            var res = new List<T>();
            for (int i = 0; i < x; i++)
            {
                if (l.Count <= 0) break;
                var s = l.ChooseRandom();
                l.Remove(s);
                res.Add(s);
            }
            return res;
        }
    }
    
    public static class ArrayExtensions
    {
        public static T GetRandomElement<T>(this T[] arr)
        {
            return arr.ChooseRandom();
        }
    }
}