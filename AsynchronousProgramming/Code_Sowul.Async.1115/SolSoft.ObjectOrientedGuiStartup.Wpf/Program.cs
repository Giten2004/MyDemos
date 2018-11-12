using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolSoft.ObjectOrientedGuiStartup.Wpf
{
	/// <summary>
	/// Get to the equivalent of default WinForms startup
	/// </summary>
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			App app = new App();

			//this applies the XAML, e.g. StartupUri, Application.Resources
			app.InitializeComponent();

			//shows the Window specified by StartupUri
			app.Run();
		}
	}
}
