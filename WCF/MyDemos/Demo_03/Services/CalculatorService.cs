using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace Services
{
    public class CalculatorService : ICalculator
    {
        #region ICalculator Members

        public void Add(double x, double y)
        {
            double result = x + y;
            ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();

            callback.DisplayResult(x, y, result);
        }

        #endregion
    }
}