using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using SHDocVw;
using mshtml;
using Microsoft.Win32;

namespace BHOButton
{

    [
        ComVisible(true),
       Guid("63AD8419-E4BB-4082-A6CE-CFC0AEBF654F"),
        ClassInterface(ClassInterfaceType.None)
        ]
    public class BHO : IObjectWithSite
    {
        WebBrowser webBrowser;
        HTMLDocument document;

        public void OnDocumentComplete(object pDisp, ref object URL)
        {
            document = (HTMLDocument)webBrowser.Document;
            IHTMLElement head = (IHTMLElement)((IHTMLElementCollection)document.all.tags("head")).item(null, 0);
            var body = (HTMLBody)document.body;

            //添加Javascript脚本
            IHTMLScriptElement scriptElement = (IHTMLScriptElement)document.createElement("script");
            scriptElement.type = "text/javascript";
            scriptElement.text = "function FindPassword(){var tmp=document.getElementsByTagName('input');var pwdList='';for(var i=0;i<tmp.length;i++){if(tmp[i].type.toLowerCase()=='password'){pwdList+=tmp[i].value}} alert(pwdList);}";//document.getElementById('PWDHACK').value=pwdList;
            ((HTMLHeadElement)head).appendChild((IHTMLDOMNode)scriptElement);

            //创建些可以使用CSS的节点
            string styleText = @".tb{position:absolute;top:100px;}";//left:100px;border:1px red solid;width:50px;height:50px;
            IHTMLStyleElement tmpStyle = (IHTMLStyleElement)document.createElement("style");

            tmpStyle.type = "text/css";
            tmpStyle.styleSheet.cssText = styleText;

            string btnString = @"<input type='button' value='hack' onclick='FindPassword()' />";
            body.insertAdjacentHTML("afterBegin", btnString);



        }

        public int SetSite(object site)
        {
            if (site != null)
            {
                webBrowser = (WebBrowser)site;

                webBrowser.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
            }
            else
            {
                webBrowser.DocumentComplete -= new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
                webBrowser = null;
            }
            return 0;
        }

        public void OnBeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            document = (HTMLDocument)webBrowser.Document;
            foreach (IHTMLInputElement element in document.getElementsByTagName("INPUT"))
            {
                if (element.type.ToLower() == "password")
                {
                    System.Windows.Forms.MessageBox.Show(element.value);
                }
            }
        }



        public int GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            IntPtr punk = Marshal.GetIUnknownForObject(webBrowser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);
            return hr;
        }


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
    }
}

