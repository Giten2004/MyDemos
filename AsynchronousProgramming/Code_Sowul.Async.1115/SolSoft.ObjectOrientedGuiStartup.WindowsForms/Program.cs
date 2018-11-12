using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
	static class Program
	{
        /// <summary>
        /// The main entry point for the application.
        /// https://msdn.microsoft.com/en-us/magazine/mt620013
        /// </summary>
        [STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
