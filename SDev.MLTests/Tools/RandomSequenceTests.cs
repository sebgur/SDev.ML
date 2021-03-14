using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDev.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDev.ML.Tests
{
    [TestClass()]
    public class RandomSequenceTests
    {
        [TestMethod()]
        public void DrawDoubleTest()
        {
            double a = RandomSequence.DrawDouble();
            double b = RandomSequence.DrawDouble();
            double c = RandomSequence.DrawDouble();
            double[] testValues = { a, b, c };
            double[] trueValues = { 0.668106465911542, 0.140907298373481, 0.125518289453126 };

            double tolerance = 1e-15;
            for (int i = 0; i < 3; i++)
                Assert.AreEqual(testValues[i], trueValues[i], tolerance);
            //CollectionAssert.AreEqual(testValues}, trueValues, 1e-12,
            //                          "oups, didn't work " + a.ToString() + "/" + b.ToString() + "/" + c.ToString());
        }
    }
}