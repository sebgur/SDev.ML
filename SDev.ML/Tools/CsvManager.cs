using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SDev.ML
{
    public static class CsvManager
    {
        public static Dictionary<string, object[]> Read(string file, char sep = '\t')
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

        public static void Write(string[] headers, object[][] data, string file, char sep = '\t')
        {
            int nFields = headers.Length;
            int nData = data.Length;
            object[][] matrix = new object[nData + 1][].SetSize(nFields);
            for (int j = 0; j < nFields; j++)
                matrix[0][j] = headers[j];

            for (int i = 0; i < nData; i++)
                for (int j = 0; j < nFields; j++)
                    matrix[1 + i][j] = data[i][j];

            Write(matrix, file, sep);
        }

        public static void Write(object[][] matrix, string file, char sep = '\t')
        {
            /// \todo Probably better to do this with StringBuilder
            string sepStr = sep.ToString();
            using (StreamWriter writer = new StreamWriter(file))
            {
                foreach (object[] row in matrix)
                {
                    string line = "";
                    for (int i = 0; i < row.Length - 1; i++)
                        line += row[i] + sepStr;
                    line += row[row.Length - 1];
                    writer.WriteLine(line);
                }
                writer.Close();
            }
        }
    }
}
