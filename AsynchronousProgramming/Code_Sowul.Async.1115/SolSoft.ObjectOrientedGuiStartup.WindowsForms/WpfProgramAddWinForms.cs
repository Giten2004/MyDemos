using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
	/// <summary>
	/// Using WPF to run the app, but still supporting Windows Forms
	/// This way is more straightforward than the reverse, even if the application is primarily WinForms-based
	/// </summary>
	class WpfProgramAddWinForms
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//the WPF application object will be the main UI loop
			System.Windows.Application wpfApplication = new System.Windows.Application
			{
				//otherwise the application will close when all WPF windows are closed
				ShutdownMode = ShutdownMode.OnExplicitShutdown 
			};
			SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

			//set the WinForms properties
			//notice how you don't need to do anything else with the Windows Forms Application (except handle exceptions)
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			
			WpfProgramAddWinForms p = new WpfProgramAddWinForms();
			p.ExitRequested += (sender, e) =>
			{
				wpfApplication.Shutdown();
			};

			Task programStart = p.StartAsync();
			HandleExceptions(programStart, wpfApplication);

			wpfApplication.Run();
		}

		private static async void HandleExceptions(Task task, System.Windows.Application wpfApplication)
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
				//e.g. to Application.DispatcherUnhandledException and AppDomain.UnhandledException
				System.Windows.MessageBox.Show(ex.ToString());

				wpfApplication.Shutdown();
			}
		}

		private readonly Form1 m_mainForm;
		private WpfProgramAddWinForms()
		{
			m_mainForm = new Form1();
			m_mainForm.FormClosed += m_mainForm_FormClosed;
		}

		public async Task StartAsync()
		{
			await Task.Yield(); //test that the synchronization context is properly set up

			SplashScreenWpf splashScreen = new SplashScreenWpf(); //not disposable, but I'm keeping the same structure
			{
				splashScreen.Closed += m_mainForm_FormClosed; //if user closes splash screen, let's quit
				splashScreen.Show();

				await m_mainForm.InitializeAsync();

				//http://blogs.msdn.com/b/wpfsdk/archive/2007/04/03/centering-wpf-windows-with-wpf-and-non-wpf-owner-windows.aspx
				WindowInteropHelper splashScreenHelper = new WindowInteropHelper(splashScreen);
				splashScreenHelper.Owner = m_mainForm.Handle;

				m_mainForm.Show();
				splashScreen.Closed -= m_mainForm_FormClosed;
				splashScreen.Close();
			}
		}

		public event EventHandler<EventArgs> ExitRequested;
		void m_mainForm_FormClosed(object sender, EventArgs e)
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
