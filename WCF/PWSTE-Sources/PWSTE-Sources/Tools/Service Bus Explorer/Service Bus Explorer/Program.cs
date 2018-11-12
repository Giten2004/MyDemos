// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Threading;

using ServiceModelEx.ServiceBus;

static class Program
{
   [STAThread]
   static void Main()
   {
      Thread.CurrentThread.Name = "Main UI Thread";

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new ExplorerForm());
   }
}