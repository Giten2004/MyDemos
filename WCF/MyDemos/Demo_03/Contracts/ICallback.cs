using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void DisplayResult(double x, double y, double result);
    }
}
