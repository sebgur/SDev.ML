using System;
using System.Collections.Generic;
using System.Linq;
using SDev.ML;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Read training set
                string trainFile = args[0];
                DataFrame df = DataFrame.Read(trainFile, sep: '\t');
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
                double eta = 0.15;
                NeuralNet model = NeuralNetFactory.Get(activation, nHiddenLayers, nNeuronsPerLayer,
                                                       inputSize, outputSize, alpha, eta);

                model.Fit(xTrain, yTrain);

                // Predict
                string predFile = args[1];
                df = DataFrame.Read(predFile, sep: '\t');
                foreach (string s in df.Headers())
                    Console.WriteLine(s);

                //double[] xPred_ = df["x"].ToDouble();
                //double[][] xPred = new double[xPred_.Length][];
                //for (int i = 0; i < xPred_.Length; i++)
                //    xPred[i] = new double[1] { xPred_[i] };
                double[][] xPred = df.SetAsDouble(new string[] { "x" });
                double[] yPred = model.Predict(xPred);

                // Output result
                string resFile = args[2];
                object[][] resData = yPred.Select(x => new object[] { x }).ToArray();
                CsvManager.Write(resData, resFile, sep: '\t');


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
