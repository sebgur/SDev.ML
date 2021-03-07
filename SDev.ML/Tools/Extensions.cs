using System;

namespace SDev.ML
{
    public static class ArrayExtensions
    {
        public static T Back<T>(this T[] l)
        {
            return l[l.Length - 1];
        }

        public static T[] Set<T>(this T[] a, T value)
        {
            int size = a.Length;
            for (int i = 0; i < size; i++)
                a[i] = value;
            return a;
        }
    }
}
