using System;

namespace SDev.ML
{
    public class Topology
    {
        #region Methods
        //
        public Topology(int[] neuronsPerLayer_)
        {
            neuronsPerLayer = neuronsPerLayer_;
            numberLayers = neuronsPerLayer.Length;
        }
        //
        public int NumberLayers()
        {
            return numberLayers;
        }
        //
        public int NumberNeuronsAtLayer(int layerIdx)
        {
            if (layerIdx >= numberLayers)
                throw new Exception("Invalid neuron index requested at layer " + layerIdx);

            return neuronsPerLayer[layerIdx];
        }
        #endregion

        #region Fields
        int[] neuronsPerLayer;
        int numberLayers;
        #endregion
    }
}
