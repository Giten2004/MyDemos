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
    /// Asynchronous with splash screen
    /// https://msdn.microsoft.com/en-us/magazine/mt620013
    /// </summary>
    class Program6
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

			Program6 p = new Program6();
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
		
		private Program6()
		{
		}

		public async Task StartAsync()
		{
			MainViewModel viewModel = new MainViewModel();
			viewModel.CloseRequested += viewModel_CloseRequested;

			EventHandler windowClosed = (sender, e) =>
			{
				viewModel.RequestClose();
			};

			SplashScreen splashScreen = new SplashScreen();  //not disposable, but I'm keeping the same structure
			{
				splashScreen.Closed += windowClosed; //if user closes splash screen, let's quit
				splashScreen.Show();

				await viewModel.InitializeAsync();

				MainWindow mainForm = new MainWindow();
				mainForm.Closed += windowClosed;
				mainForm.DataContext = viewModel;
				mainForm.Show();
				
				splashScreen.Owner = mainForm;
				splashScreen.Closed -= windowClosed;
				splashScreen.Close();
			}
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
