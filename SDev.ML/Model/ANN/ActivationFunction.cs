using System;

namespace SDev.ML
{
    public class SigmoidActivation : ActivationFunction
    {
        /// f' = f * (1 - f)
        public override double Value(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public override double Differential(double x)
        {
            double y = Math.Exp(-x);
            return y / Math.Pow(1.0 + y, 2);
        }
    }

    public class ReLUActivation : ActivationFunction
    {
        public override double Value(double x)
        {
            return Math.Max(x, 0.0);
        }

        public override double Differential(double x)
        {
            return (x < 0.0 ? 0.0 : 1.0);
        }
    }

    public class TanhActivation : ActivationFunction
    {
        public override double Value(double x)
        {
            return Math.Tanh(x);
        }

        public override double Differential(double x)
        {
            return 1.0 - Math.Tanh(x) * Math.Tanh(x);
        }
    }

    public class LinearActivation : ActivationFunction
    {
        public override double Value(double x)
        {
            if (x < min)
                return min;
            else if (x > max)
                return max;
            else
                return x;
        }

        public override double Differential(double x)
        {
            if (x < min || x > max)
                return 0.0;
            else
                return 1.0;
        }

        double min = -1.0, max = 1.0;
    }

    public abstract class ActivationFunction
    {
        public abstract double Value(double x);

        public abstract double Differential(double x);

        public static ActivationFunction Get(string type)
        {
            switch (type)
            {
                case "ReLU": return new ReLUActivation();
                case "Tanh": return new TanhActivation();
                case "Linear": return new LinearActivation();
                case "Sigmoid": return new SigmoidActivation();
                default: throw new Exception("Unknown activation function type: " + type);
            }
        }
    }
}
