using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BHOButton
{
    /// <summary>
    /// http://blog.csdn.net/ghostbear/article/details/7354214
    /// </summary>
    [
        ComVisible(true),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid("6F2DAEA4-52C0-4E54-B2F9-837EF19C60E8")
    ]
    public interface IObjectWithSite
    {
        [PreserveSig]
        int SetSite([MarshalAs(UnmanagedType.IUnknown)]object site);
        [PreserveSig]
        int GetSite(ref Guid guid, out IntPtr ppvSite);
    }
}
