using System;

namespace SDev.ML
{
    public class Neuron
    {
        #region Methods
        //
        public Neuron(int numberOutputs_, int indexInLayer_, ActivationFunction activationFunction_,
                      double alpha, double eta)
        {
            numberOutputs = numberOutputs_;
            indexInLayer = indexInLayer_;
            activationFunction = activationFunction_;
            outputConnections = new Connection[numberOutputs];
            for (int i = 0; i < numberOutputs; i++)
                outputConnections[i] = new Connection();

            Alpha = alpha;
            Eta = eta;
        }
        //
        public void FeedForward(Layer previousLayer)
        {
            double sum = 0.0;
            foreach (Neuron n in previousLayer.Neurons())
                sum += n.OutputValue * n.OutputWeight(indexInLayer);

            OutputValue = activationFunction.Value(sum);
        }
        //
        public double OutputWeight(int i)
        {
            return outputConnections[i].Weight;
        }
        //
        public void CalculateOutputGradient(double targetValue)
        {
            double delta = targetValue - OutputValue;
            gradient = delta * activationFunction.Differential(OutputValue);
        }
        //
        public void CalculateHiddenGradient(Layer nextLayer)
        {
            double dow = SumDOW(nextLayer);
            gradient = dow * activationFunction.Differential(OutputValue);
        }
        //
        public void UpdateInputWeights(ref Layer previousLayer)
        {
            // Weights to be updated are in the Connection in the neurons in the preceding layer
            previousLayer.UpdateOutputWeights(indexInLayer, gradient);
        }
        //
        private double SumDOW(Layer layer)
        {
            double sum = 0.0;
            // Sum our contributions of the errors at the nodes we feed
            for (int n = 0; n < layer.NumberNeurons() - 1; n++)
                sum += outputConnections[n].Weight * layer.Gradient(n);

            return sum;
        }
        //
        public double Gradient()
        {
            return gradient;
        }
        //
        public void UpdateOutputWeight(int neuronIdx, double neuronGradient)
        {
            double oldDeltaWeight = outputConnections[neuronIdx].DeltaWeight;
            double newDeltaWeight = Eta * OutputValue * neuronGradient + Alpha * oldDeltaWeight;
            outputConnections[neuronIdx].DeltaWeight = newDeltaWeight;
            outputConnections[neuronIdx].Weight += newDeltaWeight;
        }
        #endregion

        #region Fields
        int numberOutputs;
        Connection[] outputConnections;
        int indexInLayer;
        ActivationFunction activationFunction;
        double gradient;
        #endregion

        #region Properties
        public double OutputValue { get; set; }
        public double Alpha { get; set; }  // Momentum, multiplier of last deltaWeight, [0.0, 1.0]
        public double Eta { get; set; }  // Overall net learning rate, [0.0, 1.0]
        #endregion
    }
}
