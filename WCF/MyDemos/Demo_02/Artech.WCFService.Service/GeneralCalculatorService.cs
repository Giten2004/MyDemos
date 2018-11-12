using System;
using System.Collections.Generic;
using System.Text;

using Artech.WCFService.Contract;

namespace Artech.WCFService.Service
{
    public class GeneralCalculatorService : IGeneralCalculator
    {
        #region IGeneralCalculator Members

        public double Add(double x, double y)
        {
            return x + y;
        }

        #endregion
    }
}
