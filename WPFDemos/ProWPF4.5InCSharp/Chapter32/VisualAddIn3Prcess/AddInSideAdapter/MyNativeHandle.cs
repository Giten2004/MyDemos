using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddInSideAdapter
{
    public class MyNativeHandle : ContractBase, INativeHandleContract
    {
        private readonly INativeHandleContract _handle;

        public MyNativeHandle(INativeHandleContract handle)
        {
            _handle = handle;
        }

        #region Implement methods of interface INativeHandleContract

        public IntPtr GetHandle()
        {
            // *NOTE* This will crash if the plugin failed to give Hydra it's UI handle (as in the line of code
            // it passed in failed). This can be simulated by commented out the line of code in a plugin
            // that creates the usercontrol.
            return _handle.GetHandle();
        }

        #endregion
    }
}
