using Contract;
using HostView;
using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HostSideAdapter
{
    /// <summary>
    /// Adapts the add-in contract to the host's view of the add-in
    /// </summary>
    [HostAdapter]
    public class WPFAddIn_ContractToViewHostSideAdapter : IWPFAddInHostView
    {
        private IWPFAddInContract _wpfAddInContract;
        private ContractHandle _wpfAddInContractHandle;

        public WPFAddIn_ContractToViewHostSideAdapter(IWPFAddInContract wpfAddInContract)
        {
            // Adapt the contract (IWPFAddInContract) to the host application's
            // view of the contract (IWPFAddInHostView)
            this._wpfAddInContract = wpfAddInContract;

            // Prevent the reference to the contract from being released while the
            // host application uses the add-in
            this._wpfAddInContractHandle = new ContractHandle(wpfAddInContract);
        }

        #region Implement methods of interface IWPFAddInHostView
        public FrameworkElement GetAddInUI()
        {
            // Convert the INativeHandleContract that was passed from the add-in side
            // of the isolation boundary to a FrameworkElement
            INativeHandleContract inhc = this._wpfAddInContract.GetAddInUI();
            FrameworkElement fe = FrameworkElementAdapters.ContractToViewAdapter(inhc);
            return fe;
        }
        #endregion
    }
}
