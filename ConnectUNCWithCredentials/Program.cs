using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConnectUNCWithCredentials
{
    /// <summary>
    /// This is my git test 
    /// </summary>
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestConnect());
        }
    }
}