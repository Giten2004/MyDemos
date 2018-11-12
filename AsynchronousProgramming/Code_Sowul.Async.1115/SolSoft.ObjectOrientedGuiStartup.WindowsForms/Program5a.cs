using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
    /// <summary>
    /// Fifth refactoring, with centralized exception handling
    /// https://msdn.microsoft.com/en-us/magazine/mt620013
    /// </summary>
    class Program5a
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

			Application.ThreadException += Application_ThreadException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;


			Program5a p = new Program5a();
			p.ExitRequested += p_ExitRequested;

			Task programStart = p.StartAsync();
			HandleExceptions(programStart);

			Application.Run();
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
				MessageBox.Show(ex.ToString());

				Application.Exit();
			}
		}
		
		static void p_ExitRequested(object sender, EventArgs e)
		{
			Application.ExitThread();
		}


		static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			HandleException(e.Exception, false);
			e.SetObserved();
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception exception = e.ExceptionObject as Exception;
			if (exception == null)
			{
				if (e.ExceptionObject != null)
					exception = new Exception(e.ExceptionObject.ToString());
				else
					exception = new Exception();
			}

			HandleException(exception, false);
		}

		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleException(e.Exception, false);
		}

		static void HandleException(Exception exception, bool quit)
		{
			MessageBox.Show(exception.ToString());

			if (quit)
				Application.Exit();
		}


		private Form1 m_mainForm;
		private Program5a()
		{
			m_mainForm = new Form1();
			m_mainForm.FormClosed += m_mainForm_FormClosed;
		}

		public async Task StartAsync()
		{
			await m_mainForm.InitializeAsync();

			m_mainForm.Show();
		}

		public event EventHandler<EventArgs> ExitRequested;
		void m_mainForm_FormClosed(object sender, FormClosedEventArgs e)
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
