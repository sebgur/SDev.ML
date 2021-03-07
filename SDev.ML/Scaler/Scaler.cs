using System;

namespace SDev.ML
{
    public abstract class Scaler
    {
        /// <summary> Factory </summary>
        public static Scaler Create(string type)
        {
            switch (type)
            {
                case "Standard": return new StandardScaler();
                default: throw new Exception("Unknown scaler type: " + type);
            }
        }

        public abstract void Fit();
        public abstract void Transform();
        public abstract void FitTransform();
    }
}
