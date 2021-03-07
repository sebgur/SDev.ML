using System;

namespace SDev.ML
{
    public class Layer
    {
        #region Methods
        //
        public Layer(Neuron[] neurons_)
        {
            neurons = neurons_;
            numberNeurons = neurons.Length;
        }
        //
        public int NumberNeurons()
        {
            return numberNeurons;
        }
        //
        public Neuron[] Neurons()
        {
            return neurons;
        }
        //
        public void SetOutputValueAt(int idx, double value)
        {
            neurons[idx].OutputValue = value;
        }
        //
        public double OutputValueAt(int idx)
        {
            return neurons[idx].OutputValue;
        }
        //
        public void FeedForward(Layer previousLayer)
        {
            for (int i = 0; i < numberNeurons - 1; i++)
                neurons[i].FeedForward(previousLayer);
        }
        //
        public void CalculateOutputGradients(double[] targetValues)
        {
            for (int n = 0; n < numberNeurons - 1; n++)
                neurons[n].CalculateOutputGradient(targetValues[n]);
        }
        //
        public void CalculateHiddenGradients(Layer nextLayer)
        {
            for (int n = 0; n < numberNeurons - 1; n++)
                neurons[n].CalculateHiddenGradient(nextLayer);
        }
        //
        public void UpdateInputWeights(Layer previousLayer)
        {
            for (int n = 0; n < numberNeurons - 1; n++)
                neurons[n].UpdateInputWeights(ref previousLayer);
        }
        //
        public double Gradient(int neuronIdx)
        {
            return neurons[neuronIdx].Gradient();
        }
        //
        public void UpdateOutputWeights(int neuronIdx, double neuronGradient)
        {
            foreach (Neuron n in neurons)
                n.UpdateOutputWeight(neuronIdx, neuronGradient);
        }
        //
        public void Report(out double[] results)
        {
            results = new double[numberNeurons - 1];
            for (int i = 0; i < numberNeurons - 1; i++)
                results[i] = neurons[i].OutputValue;
        }
        #endregion

        #region Fields
        Neuron[] neurons;
        int numberNeurons;
        #endregion
    }
}
