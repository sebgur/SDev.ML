using System;
using SDev.ML;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string type = "Standard";
                Scaler scaler = Scaler.Create(type);
                scaler.Fit();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
            }
        }
    }
}
