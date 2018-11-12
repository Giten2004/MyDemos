using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ServiceModelEx
{
   static class Program
   {
      [STAThread]
      static void Main()
      {
         Application.Run(new LogbookViewerForm());
      }
   }
}