/// Implementation inspired by "grokking Deep Learning", Andrew W. Trask, ed. Manning, 2019, ch. 13

using System;
using System.Collections.Generic;
using System.Linq;

namespace SDev.ML
{
    public class Tensor
    {
        public Tensor(double[] data_, bool autograd_ = false, Tensor[] creators_ = null,
                      string creationOp_ = null,
                      string id_ = null)
        {
            data = data_;
            autograd = autograd_;
            creators = creators_;
            creationOp = creationOp_;
            id = id_;

            children = new Dictionary<string, int>();

            if (id == null)
                id = RandomGenerator.Next().ToString();

            if (creators != null)
            {
                foreach (Tensor c in creators)
                {
                    if (c.children.ContainsKey(id))
                        c.children[id] += 1;
                    else
                        c.children[id] = 1;
                }
            }
        }

        public void backward(Tensor grad_ = null, Tensor gradOrigin = null)
        {
            if (autograd)
            {
                if (gradOrigin != null)
                {
                    if (children[gradOrigin.id] == 0)
                        throw new Exception("Cannot backprop more than once");
                    else
                        children[gradOrigin.id] -= 1;
                }

                if (grad == null)
                    grad = grad_;
                else
                    grad += grad_;

                if (creators != null && (allChildrenGradsAccountedFor() || gradOrigin == null))
                {
                    switch (creationOp)
                    {
                        case "add":
                            creators[0].backward(grad, this);
                            creators[1].backward(grad, this);
                            break;
                        case "sub":
                            creators[0].backward(grad, this);
                            creators[1].backward(-grad, this);
                            break;
                        case "opp":
                            creators[0].backward(-grad, this);
                            break;
                        case "mul":
                            creators[0].backward(grad * creators[1], this);
                            creators[1].backward(grad * creators[0], this);
                            break;
                        case "div":
                            creators[0].backward(grad / creators[1], this);
                            creators[1].backward(-grad * creators[0] / creators[1] / creators[1], this);
                            break;
                        case "exp":
                            creators[0].backward(grad * Exp(creators[0]), this);
                            break;
                        case "log":
                            One logOne = new One(creators[0].dimension());
                            creators[0].backward(grad * logOne / creators[0], this);
                            break;
                        case "pow":
                            One powOne = new One(creators[1].dimension());
                            Tensor reducedPower = Pow(creators[0], creators[1] - powOne);
                            creators[0].backward(grad * creators[1] * reducedPower, this);
                            creators[1].backward(grad * this * Log(creators[0]), this);
                            break;
                        default: throw new Exception("Unknown operation in backprop: " + creationOp);
                    }
                }
            }
        }

        private bool allChildrenGradsAccountedFor()
        {
            foreach (KeyValuePair<string, int> child in children)
            {
                if (child.Value != 0)
                    return false;
            }

            return true;
        }

        #region Operations
        public static Tensor operator +(Tensor t1, Tensor t2)
        {
            double[] data = Add(t1.data, t2.data);
            if (t1.autograd && t2.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1, t2 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "add");
            }
            else
                return new Tensor(data);
        }

        public static Tensor operator -(Tensor t1, Tensor t2)
        {
            double[] data = Sub(t1.data, t2.data);
            if (t1.autograd && t2.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1, t2 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "sub");
            }
            else
                return new Tensor(data);
        }

        public static Tensor operator -(Tensor t1)
        {
            double[] data = Opp(t1.data);
            if (t1.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "opp");
            }
            else
                return new Tensor(data);
        }

        public static Tensor operator *(Tensor t1, Tensor t2)
        {
            double[] data = Mul(t1.data, t2.data);
            if (t1.autograd && t2.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1, t2 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "mul");
            }
            else
                return new Tensor(data);
        }

        public static Tensor operator /(Tensor t1, Tensor t2)
        {
            double[] data = Div(t1.data, t2.data);
            if (t1.autograd && t2.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1, t2 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "div");
            }
            else
                return new Tensor(data);
        }

        public static Tensor Exp(Tensor t1)
        {
            double[] data = Exp(t1.data);
            if (t1.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "exp");
            }
            else
                return new Tensor(data);
        }

        public static Tensor Log(Tensor t1)
        {
            double[] data = Log(t1.data);
            if (t1.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "log");
            }
            else
                return new Tensor(data);
        }

        public static Tensor Pow(Tensor t1, Tensor t2)
        {
            double[] data = Pow(t1.data, t2.data);
            if (t1.autograd && t2.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1, t2 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "pow");
            }
            else
                return new Tensor(data);
        }
        #endregion

        #region Utilities
        private static double[] Add(double[] v1, double[] v2)
        {
            int size = v1.Length;
            if (v2.Length != size)
                throw new Exception("Cannot add vectors of different lengths");

            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = v1[i] + v2[i];

            return result;
        }

        private static double[] Sub(double[] v1, double[] v2)
        {
            int size = v1.Length;
            if (v2.Length != size)
                throw new Exception("Cannot subtract vectors of different lengths");

            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = v1[i] - v2[i];

            return result;
        }

        private static double[] Opp(double[] v1)
        {
            int size = v1.Length;
            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = -v1[i];

            return result;
        }

        private static double[] Mul(double[] v1, double[] v2)
        {
            int size = v1.Length;
            if (v2.Length != size)
                throw new Exception("Cannot multiply vectors of different lengths");

            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = v1[i] * v2[i];

            return result;
        }

        private static double[] Div(double[] v1, double[] v2)
        {
            int size = v1.Length;
            if (v2.Length != size)
                throw new Exception("Cannot divide vectors of different lengths");

            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = v1[i] / v2[i];

            return result;
        }

        private static double[] Exp(double[] v1)
        {
            int size = v1.Length;
            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = Math.Exp(v1[i]);

            return result;
        }

        private static double[] Log(double[] v1)
        {
            int size = v1.Length;
            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = Math.Log(v1[i]);

            return result;
        }

        private static double[] Pow(double[] v1, double[] v2)
        {
            int size = v1.Length;
            if (v2.Length != size)
                throw new Exception("Cannot take power of vectors of different lengths");

            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = Math.Pow(v1[i], v2[i]);

            return result;
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < data.Length; i++)
                str += data[i] + ",";
            str += creationOp + ",";
            if (grad != null)
            {
                for (int i = 0; i < grad.data.Length; i++)
                    str += grad.data[i] + ",";
            }

            return str;
        }

        public Tensor Gradient()
        {
            return grad;
        }

        public int dimension()
        {
            return data.Length;
        }
        #endregion

        #region Fields
        double[] data;
        Tensor[] creators;
        string creationOp;
        Tensor grad;
        bool autograd;
        Dictionary<string, int> children;
        #endregion

        #region Properties
        public string id { get; private set; }
        #endregion
    }

    public class One : Tensor
    {
        public One(int dim) : base (new double[dim].Set(1), autograd_: false)
        {
        }
    }
}
