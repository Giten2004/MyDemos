using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using mshtml;
using SHDocVw;
using Microsoft.Win32;

namespace BHOHelloWorld
{
    [
        ComVisible(true),
        Guid("D539BE65-BBF4-4920-B9AC-9FF9AAD6C9E7"),
        ClassInterface(ClassInterfaceType.None)
    ]
    public class BHO : IObjectWithSite
    {
        private WebBrowser _webBrowser;
        private HTMLDocument _document;

        public static string BHOKEYNAME = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        [ComRegisterFunction]
        public static void RegisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);

            if (registryKey == null)
                registryKey = Registry.LocalMachine.CreateSubKey(BHOKEYNAME);

            string guid = type.GUID.ToString("B");
            RegistryKey ourKey = registryKey.OpenSubKey(guid);

            if (ourKey == null)
                ourKey = registryKey.CreateSubKey(guid);

            ourKey.SetValue("Alright", 1);
            registryKey.Close();
            ourKey.Close();
        }

        [ComUnregisterFunction]
        public static void UnregisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);
            string guid = type.GUID.ToString("B");

            if (registryKey != null)
                registryKey.DeleteSubKey(guid, false);
        }

        #region Implement methods of interface IObjectWithSite

        public int SetSite(object site)
        {
            if (site != null)
            {
                _webBrowser = (WebBrowser)site;
                _webBrowser.DocumentComplete += OnDocumentComplete;
                _webBrowser.BeforeNavigate2 += OnBeforeNavigate2;
            }
            else
            {
                _webBrowser.DocumentComplete -= OnDocumentComplete;
                _webBrowser.BeforeNavigate2 -= OnBeforeNavigate2;
                _webBrowser = null;
            }

            return 0;
        }

        public int GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            IntPtr punk = Marshal.GetIUnknownForObject(_webBrowser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);

            return hr;
        }

        #endregion

        private void OnDocumentComplete(object pDisp, ref object URL)
        {
            try
            {
                LogHelper.LogInfo("OnDocumentComplete");
                _document = (HTMLDocument)_webBrowser.Document;

                foreach (IHTMLInputElement tempElement in _document.getElementsByTagName("INPUT"))
                {
                    var message = tempElement.name != null ? tempElement.name : "it sucks, no name, try id" + ((IHTMLElement)tempElement).id;

                    System.Windows.Forms.MessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("OnDocumentComplete", ex);
            }
        }

        public void OnBeforeNavigate2(object pDisp, ref object URL, ref object Flags,
            ref object TargetFrameName, ref object PostData, ref object Headers,
            ref bool Cancel)
        {
            try
            {
                LogHelper.LogInfo("OnBeforeNavigate2");
                _document = (HTMLDocument)_webBrowser.Document;

                foreach (IHTMLInputElement tempElement in _document.getElementsByTagName("INPUT"))
                {
                    if (tempElement.type.ToLower() == "password")
                    {
                        System.Windows.Forms.MessageBox.Show(tempElement.value);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("OnBeforeNavigate2", ex);
            }

        }
    }
}
