using System;
using System.IO;
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

        public static DataFrame Read(string file, char sep = ',')
        {
            Dictionary<string, object[]> dic = CsvManager.Read(file, sep);
            DataFrame df = new DataFrame(dic);
            return df;
        }

        public string[] Headers()
        {
            return dic.Keys.ToArray();
        }

        public object[] this[string key]
        {
            get { return this.dic[key]; }
            set { this.dic[key] = value; }
        }

        Dictionary<string, object[]> dic;
    }

    public static class CsvManager
    {
        public static Dictionary<string, object[]> Read(string file, char sep = ',')
        {
            string[][] lines = File.ReadAllLines(file).Select(x => x.Split('\n')).ToArray();
            string[][] csv = lines.Select(x => x[0].Split(sep)).ToArray();
            int nCSV = csv.Length;
            if (nCSV < 1)
                throw new Exception("No fields found in file");
            if (nCSV < 2)
                throw new Exception("No data found in file");

            string[] headers = csv[0];
            int nHeaders = headers.Length;
            int nData = nCSV - 1;
            object[][] data = new object[nHeaders][];
            for (int i = 0; i < nHeaders; i++)
                data[i] = new object[nData];
            for (int j = 1; j < nCSV; j++)
            {
                for (int i = 0; i < nHeaders; i++)
                    data[i][j - 1] = csv[j][i];
            }

            Dictionary<string, object[]> dic = new Dictionary<string, object[]>();
            for (int i = 0; i < nHeaders; i++)
                dic.Add(headers[i], data[i]);

            return dic;
        }
    }
}
