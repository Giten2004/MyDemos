using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    /// <summary>
    /// Defines the services that an add-in will provide to a host application
    /// </summary>
    [AddInContract]
    public interface IWPFAddInContract : IContract
    {
        // Return a UI to the host application
        INativeHandleContract GetAddInUI();
    }
}
