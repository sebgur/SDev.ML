using System;

/// Strongly inspired by David Miller's online video at https://vimeo.com/19569529

namespace SDev.ML
{
    public class NeuralNet
    {
        #region Methods
        //
        public NeuralNet(Topology topology, ActivationFunction activationFunction_, double alpha = 0.5, double eta = 0.15)
        {
            RandomSequence.Reset();
            numberLayers = topology.NumberLayers();
            if (numberLayers < 2)
                throw new Exception("Number of layers should be at least 2");

            layers = new Layer[numberLayers];
            ActivationFunction activationFunction = activationFunction_;
            for (int layerIdx = 0; layerIdx < numberLayers; layerIdx++)
            {
                int nNeurons = topology.NumberNeuronsAtLayer(layerIdx);
                Neuron[] neurons = new Neuron[nNeurons + 1];
                int nOutputs = (layerIdx == numberLayers - 1 ? 0 : topology.NumberNeuronsAtLayer(layerIdx + 1));
                // Main neurons
                for (int neuronIdx = 0; neuronIdx < nNeurons + 1; neuronIdx++)
                    neurons[neuronIdx] = new Neuron(nOutputs, neuronIdx, activationFunction, alpha, eta);

                // Force the bias neuron's output value to 1.0
                neurons.Back().OutputValue = 1.0;

                layers[layerIdx] = new Layer(neurons);
            }
        }
        //
        public void Fit(double[][] samplePoints, double[][] sampleValues)
        {
            for (int e = 0; e < nEpochs; e++)
            {
                for (int i = 0; i < samplePoints.Length; i++)
                {
                    FeedForward(samplePoints[i]);
                    BackPropagation(sampleValues[i]);
                }
            }
        }
        //
        public double[] Predict(double[][] x)
        {
            int nPred = x.Length;
            double[] yPred = new double[nPred];
            for (int i = 0; i < nPred; i++)
            {
                FeedForward(x[i]);
                double[] results;
                Report(out results);
                yPred[i] = results[0];
            }

            return yPred;
        }
        //
        public void FeedForward(double[] inputValues)
        {
            // Assign input values to input neurons
            if (inputValues.Length != layers[0].NumberNeurons() - 1) // Minus the bias neuron
                throw new Exception("Inconsistent size of inputs");

            for (int i = 0; i < inputValues.Length; i++)
                layers[0].SetOutputValueAt(i, inputValues[i]);

            // Forward propagation
            for (int layerIdx = 1; layerIdx < numberLayers; layerIdx++)
                layers[layerIdx].FeedForward(layers[layerIdx - 1]);
        }
        //
        public void BackPropagation(double[] targetValues)
        {
            // Calculate overall net error
            Layer outputLayer = layers.Back();
            double error = 0.0;
            int dimTarget = targetValues.Length;
            if (outputLayer.NumberNeurons() - 1 != dimTarget)
                throw new Exception("Inconsistent side in targets");

            for (int n = 0; n < dimTarget; n++)
                error += Math.Pow(targetValues[n] - outputLayer.OutputValueAt(n), 2);
            error = Math.Sqrt(error / dimTarget);

            // Recent average measurement
            recentAverageError = (recentAverageError * recentAverageSmoothingFactor + error) / (recentAverageSmoothingFactor + 1.0);

            // Calculate output layer gradients
            outputLayer.CalculateOutputGradients(targetValues);

            // Calculate gradients on hidden layers
            for (int layerIdx = numberLayers - 2; layerIdx > 0; --layerIdx)
            {
                Layer hiddenLayer = layers[layerIdx];
                Layer nextLayer = layers[layerIdx + 1];
                hiddenLayer.CalculateHiddenGradients(nextLayer);
            }

            // For all layers from outputs to first hidden layer, update weights
            for (int layerIdx = numberLayers - 1; layerIdx > 0; --layerIdx)
            {
                Layer layer = layers[layerIdx];
                Layer previousLayer = layers[layerIdx - 1];
                layer.UpdateInputWeights(previousLayer);
            }
        }
        //
        public void Report(out double[] results)
        {
            layers.Back().Report(out results);
        }
        #endregion

        #region Fields
        Layer[] layers;
        int numberLayers;
        double recentAverageError, recentAverageSmoothingFactor = 1.0;
        int nEpochs = 10;
        #endregion
    }
}
