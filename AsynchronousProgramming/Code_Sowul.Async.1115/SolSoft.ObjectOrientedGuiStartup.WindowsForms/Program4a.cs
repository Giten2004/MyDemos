using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
    /// <summary>
    /// Fourth refactoring, with synchronization context
    /// Still buggy, don't use!
    /// https://msdn.microsoft.com/en-us/magazine/mt620013
    /// </summary>
    class Program4a
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

			Program4a p = new Program4a();
			p.ExitRequested += p_ExitRequested;

			Task programStart = p.StartAsync();

			Application.Run();
		}

		static void p_ExitRequested(object sender, EventArgs e)
		{
			Application.ExitThread();
		}

		private readonly Form1 m_mainForm;
		private Program4a()
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
