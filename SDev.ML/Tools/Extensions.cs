using System;
using System.Linq;

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

        public static double[] ToDouble(this object[] a)
        {
            return a.Select(x => Convert.ToDouble(x)).ToArray();
        }

        //
        public static T[][] SetSize<T>(this T[][] a, int size)
        {
            int mainSize = a.Length;
            for (int i = 0; i < mainSize; i++)
                a[i] = new T[size];
            return a;
        }
    }
}
