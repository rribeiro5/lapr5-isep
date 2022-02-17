using System.Collections;
using System.Collections.Generic;

namespace DDDSample1.Utils{
    public static class IsListNullOrEmptyExtension
    {
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            if (source != null)
            {
                foreach (object obj in source)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source != null)
            {
                foreach (T obj in source)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool GreaterThan(this IEnumerable source, int size)
        {   
            int length = 0;
            if (source != null)
            {
                foreach (object obj in source)
                {
                    length++;
                }
            }

            return length > size;
        }

        public static bool GreaterThan<T>(this IEnumerable<T> source, int size)
        {   
            int length = 0;
            if (source != null)
            {
                foreach (object obj in source)
                {
                    length++;
                }
            }

            return length > size;
        }
        


    }
}
