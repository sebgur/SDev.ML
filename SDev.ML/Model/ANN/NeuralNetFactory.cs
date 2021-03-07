using System;

namespace SDev.ML
{
    public class NeuralNetFactory
    {
        public static NeuralNet Get(string activationType, int numberHiddenLayers, int neuronsPerLayer,
                                    int inputDimension, int outputDimension, double alpha, double eta)
        {
            int[] neuronStructure = new int[numberHiddenLayers + 2].Set(neuronsPerLayer);
            neuronStructure[0] = inputDimension;
            neuronStructure[numberHiddenLayers + 1] = outputDimension;
            Topology topology = new Topology(neuronStructure);
            ActivationFunction activationFunction = ActivationFunction.Get(activationType);
            return new NeuralNet(topology, activationFunction, alpha, eta);
        }
    }
}
