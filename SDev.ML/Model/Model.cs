using System;

namespace SDev.ML
{
    public abstract class Model
    {
        public abstract void Fit();
        public abstract void Predict();
        public abstract void FitPredict();
    }
}
