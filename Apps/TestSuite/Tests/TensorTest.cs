using System;
using System.Collections.Generic;
using SDev.ML;

namespace TestSuite
{
    public class TensorTest
    {
        public static void Test(string[] args)
        {
            Tensor x = new Tensor(new double[] { 2.5, 3.5 }, autograd_: true);
            //Tensor y = new Tensor(new double[] { 1.2, 4.2 }, autograd_: true);
            Tensor pow2 = new Tensor(new double[] { 2, 2 }, autograd_: true);
            //Tensor pow3 = new Tensor(new double[] { 3, 3 }, autograd_: true);
            Tensor X = Tensor.Pow(x, pow2);
            //Tensor Y = Tensor.Pow(y, pow3);

            //Tensor f = X * X;
            Tensor f = Tensor.Pow(X, pow2);
            f.backward(new Tensor(new double[] { 1, 1 }));

            //Console.WriteLine(b);
            Console.WriteLine(x.Gradient());
            Console.WriteLine(pow2.Gradient());
        }
    }
}
