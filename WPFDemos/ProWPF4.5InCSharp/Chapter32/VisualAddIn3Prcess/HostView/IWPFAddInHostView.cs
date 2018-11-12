using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HostView
{
    /// <summary>
    /// Defines the host's view of the add-in
    /// </summary>
    public interface IWPFAddInHostView
    {
        // The view returns as a class that directly or indirectly derives from 
        // FrameworkElement and can subsequently be displayed by the host 
        // application by embedding it as content or sub-content of a UI that is 
        // implemented by the host application.
        FrameworkElement GetAddInUI();
    }
}
