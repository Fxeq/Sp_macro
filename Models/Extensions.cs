using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp_macro
{
    public static class Extensions
    {
        public static T[] subArray<T>(this T[] data, int first, int last)
        {
            T[] result = new T[last - first];
            Array.Copy(data, first, result, 0, last - first);
            return result;
        }

        public static bool isEmpty<T>(this T[] data)
        {
            return data == null || data.Length == 0;
        }

        public static bool isNotEmpty<T>(this T[] data)
        {
            return !data.isEmpty();
        }

        public static bool isEmpty<T>(this ICollection<T> data)
        {
            return data?.Count == 0;
        }

        public static bool isNotEmpty<T>(this Stack<T> data)
        {
            return data?.Count != 0;
        }

        public static bool isNotEmpty(this string value)
        {
            return value != null && !value.Equals("");
        }

        public static bool isEmpty(this string value)
        {
            return value == null || value.Equals("");
        }

        public static T get<T>(this T[] data, int index)
        {
            return index >= data.Length ? default(T) : data[index];
        }

        public static IList<R> map<T,R>(this IList<T> data, Func<T, R> mapFunction)
        {
            IList<R> newList = new List<R>();
            foreach(T item in data)
            {
                newList.Add(mapFunction(item));
            }

            return newList;
        }


        public static IList<T> startFrom<T>(this IList<T> data, int index)
        {
            IList<T> newList = new List<T>();
            for (int i = index; i < data.Count; i++)
            {
                newList.Add(data[i]);
            }

            return newList;
        }
        
    }
}
