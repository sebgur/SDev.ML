using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDev.ML
{
    public class RandomGenerator
    {
        public static int Next()
        {
            return rng.Next();
        }

        #region Fields
        static Random rng = new Random(42);
        #endregion
    }
}
