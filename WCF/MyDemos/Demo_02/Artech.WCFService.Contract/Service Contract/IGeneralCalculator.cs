using System;
using System.Collections.Generic;
using System.Text;

using System.ServiceModel;

namespace Artech.WCFService.Contract
{
    [ServiceContract]
    public interface IGeneralCalculator
    {
        [OperationContract]
        double Add(double x, double y);
    }
}
