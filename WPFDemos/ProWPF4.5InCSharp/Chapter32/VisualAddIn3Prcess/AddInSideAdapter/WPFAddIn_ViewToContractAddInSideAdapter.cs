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
        //private readonly Lazy<AddInSideAppDispatcher> _ad;
        private readonly AddInSideAppDispatcher _ad;

        public WPFAddIn_ViewToContractAddInSideAdapter(IWPFAddInView wpfAddInView)
        {
            // Adapt the add-in view of the contract (IWPFAddInView) 
            // to the contract (IWPFAddInContract)
            _wpfAddInView = wpfAddInView;

            //_ad = new Lazy<AddInSideAppDispatcher>();
            _ad = new AddInSideAppDispatcher();
        }

        #region Implement methods of interface IWPFAddInContract
        //public INativeHandleContract GetAddInUI()
        //{
        //    // Convert the FrameworkElement from the add-in to an INativeHandleContract 
        //    // that will be passed across the isolation boundary to the host application.
        //    FrameworkElement fe = this._wpfAddInView.GetAddInUI();
        //    INativeHandleContract inhc = FrameworkElementAdapters.ViewToContractAdapter(fe);
        //    return inhc;
        //}

        public INativeHandleContract GetAddInUI()
        {
            // Convert the FrameworkElement from the add-in to an INativeHandleContract 
            // that will be passed across the isolation boundary to the host application.
            System.AddIn.Contract.INativeHandleContract value = null;
            _ad.DoWork(
                delegate
                {
                    //This special adapter has one purposes: 
                    //1. Cut off the standard WPF tabbing support to ensure that it doesn't do its work on the wrong thread (by ensuring it doesn't do it at all)
                    FrameworkElement fe = _wpfAddInView.GetAddInUI();
                    INativeHandleContract inhc = FrameworkElementAdapters.ViewToContractAdapter(fe);
                    value = new MyNativeHandle(inhc);
                }
                );
            return value;
        }
        #endregion
    }
}
