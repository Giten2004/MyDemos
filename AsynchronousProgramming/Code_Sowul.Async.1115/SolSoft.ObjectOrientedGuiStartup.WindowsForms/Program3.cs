using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
    /// <summary>
    /// Third refactoring: extract the hosting boilerplate from the class
    /// https://msdn.microsoft.com/en-us/magazine/mt620013
    /// </summary>
    class Program3
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Program3 p = new Program3();
			p.ExitRequested += p_ExitRequested;

			p.Start();

			Application.Run();
		}

		static void p_ExitRequested(object sender, EventArgs e)
		{
			Application.ExitThread();
		}

		private readonly Form1 m_mainForm;
		private Program3()
		{
			m_mainForm = new Form1();
			m_mainForm.FormClosed += m_mainForm_FormClosed;
		}

		public void Start()
		{
			m_mainForm.Initialize();
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
