using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDev.ML;

namespace TestSuite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TensorTest.Test(args);
                //TrainingSetTest.Test(args);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
            }
        }
    }
}
