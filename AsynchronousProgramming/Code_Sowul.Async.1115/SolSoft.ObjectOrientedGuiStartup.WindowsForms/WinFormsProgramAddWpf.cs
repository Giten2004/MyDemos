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
	/// Using WinForms to run the app, but still supporting WPF.  
	/// This would seemingly be the scenario when adding WPF to a legacy application...however,
	/// Even if that's what you're doing, see WpfAddWinForms -- that way is more straightforward
	/// </summary>
	class WinFormsProgramAddWpf
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//The WinForms Application will be the main UI loop
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

			//it's more awkward to have WinForms run the show; in certain cirumstances, if you don't change the WPF Application shutdown mode,
			//the application will be destroyed when that window is closed (because of the default "OnLastWindowClose")
			//then when you open a second WPF window...boom
			System.Windows.Application wpfApplication = new System.Windows.Application
			{
				//comment out this line to test, then click the button on the form to show another WPF window
				ShutdownMode = ShutdownMode.OnExplicitShutdown
			};
			
			WinFormsProgramAddWpf p = new WinFormsProgramAddWpf();
			p.ExitRequested += (sender, e) =>
			{
				System.Windows.Forms.Application.ExitThread();
			};

			Task programStart = p.StartAsync();
			HandleExceptions(programStart);


			System.Windows.Forms.Application.Run();
			//now the wpf application can also shut down
			wpfApplication.Shutdown();
		}

		private static async void HandleExceptions(Task task)
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
				//e.g. to Application.ThreadException and AppDomain.UnhandledException
				System.Windows.Forms.MessageBox.Show(ex.ToString());

				System.Windows.Forms.Application.ExitThread();
			}
		}

		private Form1 m_mainForm;
		private WinFormsProgramAddWpf()
		{
			m_mainForm = new Form1();
			m_mainForm.FormClosed += m_mainForm_FormClosed;
		}

		public async Task StartAsync()
		{
			await Task.Yield(); //test that the synchronization context is properly set up

			SplashScreenWpf splashScreen = new SplashScreenWpf();  //not disposable, but I'm keeping the same structure
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


			//This lets you test
			Button b = new Button
			{
				Text = "Show WPF window", 
				AutoSize = true,
			};
			m_mainForm.Controls.Add(b);
			b.Click += (sender, e) =>
			{
				new SplashScreenWpf().Show();
			};
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
