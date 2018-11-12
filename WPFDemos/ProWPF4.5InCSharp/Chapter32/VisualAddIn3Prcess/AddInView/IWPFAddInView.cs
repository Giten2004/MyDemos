using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AddInView
{
    /// <summary>
    /// Defines the add-in's view of the contract
    /// </summary>
    [AddInBase]
    public interface IWPFAddInView
    {
        // The add-in's implementation of this method will return
        // a UI type that directly or indirectly derives from 
        // FrameworkElement.
        FrameworkElement GetAddInUI();
    }
}
