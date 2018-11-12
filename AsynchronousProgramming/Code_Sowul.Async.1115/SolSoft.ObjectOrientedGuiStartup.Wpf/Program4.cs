using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SolSoft.ObjectOrientedGuiStartup.Wpf
{
	/// <summary>
	/// Fourth refactoring: first attempt at asynchronous initialization
	/// BUGGY, don't use!
	/// </summary>
	class Program4
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

			Program4 p = new Program4();
			p.ExitRequested += (sender, e) =>
			{
				app.Shutdown();
			};
			Task programStart = p.StartAsync();

			app.Run();
		}

		private Program4()
		{
		}

		public async Task StartAsync()
		{
			MainViewModel viewModel = new MainViewModel();
			viewModel.CloseRequested += viewModel_CloseRequested;
			await viewModel.InitializeAsync();

			//this will be on a threadpool thread instead of a ui thread, and won't work
			//it will throw an exception because it's not an STA thread
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
