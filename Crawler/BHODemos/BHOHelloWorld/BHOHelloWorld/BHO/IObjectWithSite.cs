using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BHOHelloWorld
{
    /// <summary>
    /// https://www.codeproject.com/articles/19971/how-to-attach-to-browser-helper-object-bho-with-c
    /// 
    /// https://www.codeproject.com/articles/350432/bho-development-using-
    /// </summary>
    [
        ComVisible(true),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid("B275B3AC-BA6A-4148-8F3F-A515EE832DA7")
    ]
    public interface IObjectWithSite
    {
        [PreserveSig]
        int SetSite([MarshalAs(UnmanagedType.IUnknown)]object site);
        [PreserveSig]
        int GetSite(ref Guid guid, out IntPtr ppvSite);
    }
}
