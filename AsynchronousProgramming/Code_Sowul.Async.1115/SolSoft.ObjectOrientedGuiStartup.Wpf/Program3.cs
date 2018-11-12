using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SolSoft.ObjectOrientedGuiStartup.Wpf
{
	/// <summary>
	/// Third refactoring: extract the hosting boilerplate from the class
	/// </summary>
	class Program3
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			AppNoStartupUri app = new AppNoStartupUri
			{
				ShutdownMode = ShutdownMode.OnExplicitShutdown
			};
			app.InitializeComponent();

			Program3 p = new Program3();
			p.ExitRequested += (sender, e) =>
			{
				app.Shutdown();
			};
			p.Start();

			app.Run();
		}

		private Program3()
		{

		}

		public void Start()
		{
			MainViewModel viewModel = new MainViewModel();
			viewModel.CloseRequested += viewModel_CloseRequested;
			viewModel.Initialize();

			MainWindow mainForm = new MainWindow();
			mainForm.Closed += (sender, e) =>
			{
				viewModel.RequestClose();
			};

			mainForm.DataContext = viewModel;
			mainForm.Show();
		}

		public event EventHandler<EventArgs> ExitRequested;
		void viewModel_CloseRequested(object sender, EventArgs e)
		{
			OnExitRequested(EventArgs.Empty);
		}

		protected virtual void OnExitRequested(EventArgs e)
		{
			if (ExitRequested != null)
				ExitRequested(this, e);
		}
	}
}
