using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

			Program5 p = new Program5();
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

		private Form1 m_mainForm;
		private Program5()
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
