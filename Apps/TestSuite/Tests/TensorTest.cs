using System;
using System.Collections.Generic;
using SDev.ML;

namespace TestSuite
{
    public class TensorTest
    {
        public static void Test(string[] args)
        {
            Tensor a = new Tensor(new double[] { 1, 2, 3, 4, 5 });
            Tensor b = new Tensor(new double[] { 2, 2, 2, 2, 2 });
            Tensor c = new Tensor(new double[] { 5, 4, 3, 2, 1 });
            Tensor d = a + b;
            Tensor e = b + c;
            Tensor f = d + e;
            f.backward(new Tensor(new double[] { 1, 1, 1, 1, 1 }));

            Console.WriteLine(b.Gradient());
        }
    }
}
