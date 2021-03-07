using System;

namespace SDev.ML
{
    public class StandardScaler : Scaler
    {
        public override void Fit()
        {
            string currentMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            throw new NotImplementedException("Method not implemented: " + currentMethod);
        }

        public override void FitTransform()
        {
            throw new NotImplementedException();
        }

        public override void Transform()
        {
            throw new NotImplementedException();
        }
    }
}
