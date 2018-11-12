using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Artech.WCFService.Contract;

namespace Artech.WCFService.Client
{
    class GeneralCalculatorClient : ClientBase<IGeneralCalculator>, IGeneralCalculator
    {
        public GeneralCalculatorClient()
            : base()
        { }

        public GeneralCalculatorClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        { }

        public GeneralCalculatorClient(Binding binding, EndpointAddress address)
            : base(binding, address)
        { }

        #region IGeneralCalculator Members

        public double Add(double x, double y)
        {
            return this.Channel.Add(x, y);
        }

        #endregion
    }
}
