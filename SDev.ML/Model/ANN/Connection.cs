using System;

namespace SDev.ML
{
    public class Connection
    {
        #region Methods
        //
        public Connection()
        {
            Weight = RandomGenerator.Draw();
            DeltaWeight = 0.0;
            //DeltaWeight = Constant.MACHINE_MAX;
        }
        #endregion

        #region Properties
        public double Weight { get; set; }
        public double DeltaWeight { get; set; }
        #endregion
    }
}
