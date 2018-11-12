using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SolSoft.ObjectOrientedGuiStartup.Wpf
{
    /// <summary>
    /// Fifth refactoring, with exception handling
    /// https://msdn.microsoft.com/en-us/magazine/mt620013
    /// </summary>
    class Program5
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
			SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

			Program5 p = new Program5();
			p.ExitRequested += (sender, e) =>
			{
				app.Shutdown();
			};
			Task programStart = p.StartAsync();
			HandleExceptions(programStart, app);

			app.Run();
		}

		private static async void HandleExceptions(Task task, Application wpfApplication)
		{
			try
			{
				await Task.Yield(); //ensure this runs as a continuation
				await task;
			}
			catch (Exception ex)
			{
				//deal with exception, either with message box
				//or delegating to general exception handling logic you may have wired up 
				//e.g. to app.DispatcherUnhandledException and AppDomain.UnhandledException
				MessageBox.Show(ex.ToString());

				wpfApplication.Shutdown();
			}
		}

		private Program5()
		{
		}

		public async Task StartAsync()
		{
			MainViewModel viewModel = new MainViewModel();
			viewModel.CloseRequested += viewModel_CloseRequested;
			await viewModel.InitializeAsync();

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
