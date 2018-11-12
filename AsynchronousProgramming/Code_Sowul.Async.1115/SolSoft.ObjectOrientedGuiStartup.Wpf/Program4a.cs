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
	/// Fourth refactoring, with synchronization context
	/// Still buggy, don't use!
	/// </summary>
	class Program4a
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			App app = new App
			{
				ShutdownMode = ShutdownMode.OnExplicitShutdown
			};
			app.InitializeComponent(); 
			SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

			Program4a p = new Program4a();
			p.ExitRequested += (sender, e) =>
			{
				app.Shutdown();
			};
			Task programStart = p.StartAsync();

			app.Run();
		}

		private Program4a()
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
