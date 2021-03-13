using System;
using System.Collections.Generic;
using SDev.ML;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string file = args[0];

                // Read dataset
                DataFrame df = DataFrame.Read(file, sep: '\t');
                foreach (string s in df.Headers())
                    Console.WriteLine(s);

                double[] x = df["x"].ToDouble();
                double[] y = df["y"].ToDouble();

                // Prepare training set
                int nTrain = x.Length;
                double[][] xTrain = new double[nTrain][];
                double[][] yTrain = new double[nTrain][];
                for (int i = 0; i < nTrain; i++)
                {
                    xTrain[i] = new double[] { x[i] };
                    yTrain[i] = new double[] { y[i] };
                }

                // Set up model
                string activation = "Relu";
                int nHiddenLayers = 0;
                int nNeuronsPerLayer = 2;
                int inputSize = xTrain[0].Length;
                int outputSize = yTrain[0].Length;
                double alpha = 0.5;
                double eta = 0.5;
                NeuralNet model = NeuralNetFactory.Get(activation, nHiddenLayers, nNeuronsPerLayer,
                                                       inputSize, outputSize, alpha, eta);
                //model.BackPropagation();
                string ss = "";
                ss += "";
                //string type = "Standard";
                //Scaler scaler = Scaler.Create(type);
                //scaler.Fit();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
            }
        }
    }
}
