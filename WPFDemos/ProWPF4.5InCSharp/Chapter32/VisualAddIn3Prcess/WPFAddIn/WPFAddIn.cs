using AddInView;
using System;
using System.AddIn;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFAddIn
{
    /// <summary>
    /// Add-In implementation
    /// </summary>
    [AddIn("WPF Add-In 1")]
    public class WPFAddIn : IWPFAddInView
    {
        public FrameworkElement GetAddInUI()
        {
            Debug.WriteLine("GetAddInUIGetAddInUIGetAddInUIGetAddInUI");
            // Return add-in UI
            return new AddInUI();
        }
    }
}
