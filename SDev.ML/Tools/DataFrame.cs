using System;
using System.Collections.Generic;
using System.Linq;

namespace SDev.ML
{
    public class DataFrame
    {
        public DataFrame()
        {
            dic = new Dictionary<string, object[]>();
        }

        public DataFrame(Dictionary<string, object[]> dic_)
        {
            dic = dic_;
        }

        public static DataFrame Read(string file, char sep = '\t')
        {
            Dictionary<string, object[]> dic = CsvManager.Read(file, sep);
            DataFrame df = new DataFrame(dic);
            return df;
        }

        public string[] Headers()
        {
            return dic.Keys.ToArray();
        }

        public double[][] SetAsDouble(string[] keys)
        {
            int dataSize = dic[keys[0]].Length;
            double[][] setAsDouble = new double[dataSize][].SetSize(keys.Length);
            for (int j = 0; j < keys.Length; j++)
            {
                double[] fieldData = dic[keys[j]].ToDouble();
                if (fieldData.Length != dataSize)
                    throw new Exception("Inconsistent data size in data frame");
                for (int i = 0; i < dataSize; i++)
                    setAsDouble[i][j] = fieldData[i];
            }

            return setAsDouble;
        }

        public object[] this[string key]
        {
            get { return this.dic[key]; }
            set { this.dic[key] = value; }
        }

        Dictionary<string, object[]> dic;
    }
}
