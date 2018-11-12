using AddInView;
using Contract;
using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AddInSideAdapter
{
    /// <summary>
    /// Adapts the add-in's view of the contract to the add-in contract
    /// </summary>
    [AddInAdapter]
    public class WPFAddIn_ViewToContractAddInSideAdapter : ContractBase, IWPFAddInContract
    {
        private IWPFAddInView _wpfAddInView;

        public WPFAddIn_ViewToContractAddInSideAdapter(IWPFAddInView wpfAddInView)
        {
            // Adapt the add-in view of the contract (IWPFAddInView) 
            // to the contract (IWPFAddInContract)
            this._wpfAddInView = wpfAddInView;
        }

        #region Implement methods of interface IWPFAddInContract
        public INativeHandleContract GetAddInUI()
        {
            // Convert the FrameworkElement from the add-in to an INativeHandleContract 
            // that will be passed across the isolation boundary to the host application.
            FrameworkElement fe = this._wpfAddInView.GetAddInUI();
            INativeHandleContract inhc = FrameworkElementAdapters.ViewToContractAdapter(fe);
            return inhc;
        }
        #endregion
    }
}
