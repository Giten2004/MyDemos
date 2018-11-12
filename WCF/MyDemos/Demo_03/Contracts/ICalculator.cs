using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract(Namespace = "http://www.artech.com/", CallbackContract = typeof(ICallback))]
    public interface ICalculator
    {
        [OperationContract(IsOneWay = true)]
        void Add(double x, double y);
    }
}
