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
                    if (creationOp == "add")
                    {
                        creators[0].backward(grad, this);
                        creators[1].backward(grad, this);
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
            double[] data = add(t1.data, t2.data);
            if (t1.autograd && t2.autograd)
            {
                Tensor[] newCreators = new Tensor[] { t1, t2 };
                return new Tensor(data, autograd_: true, creators_: newCreators, creationOp_: "add");
            }
            else
                return new Tensor(data);
        }
        #endregion

        #region Utilities
        private static double[] add(double[] v1, double[] v2)
        {
            int size = v1.Length;
            if (v2.Length != size)
                throw new Exception("Cannot add vectors of different lengths");

            double[] result = new double[size];
            for (int i = 0; i < size; i++)
                result[i] = v1[i] + v2[i];

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
}
