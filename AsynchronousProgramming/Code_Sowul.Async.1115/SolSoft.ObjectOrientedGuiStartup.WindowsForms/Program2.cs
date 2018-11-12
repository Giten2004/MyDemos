using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
	/// <summary>
	/// Second refactoring: separate message loop
	/// </summary>
	class Program2
	{
        /// <summary>
        /// The main entry point for the application.
        /// https://msdn.microsoft.com/en-us/magazine/mt620013
        /// </summary>
        [STAThread]
		static void Main()
		{
			Program2 p = new Program2();
			p.Run();
		}

		private readonly Form1 m_mainForm;
		private Program2()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			m_mainForm = new Form1();
			m_mainForm.FormClosed += m_mainForm_FormClosed;
		}

		public void Run()
		{
			m_mainForm.Initialize();
			m_mainForm.Show();
			Application.Run();
		}

		void m_mainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.ExitThread();
		}
	}
}
