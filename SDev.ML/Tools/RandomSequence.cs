using System;

namespace SDev.ML
{
    public static class RandomSequence
    {
        #region Methods

        public static void Reset()
        {
            rnd = new Random(42);
        }

        public static double DrawDouble()
        {
            return rnd.NextDouble();
        }

        public static int DrawInt()
        {
            return rnd.Next();
        }

        #endregion

        static Random rnd = new Random(42);
    }
}
