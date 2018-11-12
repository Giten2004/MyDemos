using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
	/// <summary>
	/// First refactoring: closer to object-oriented
	/// </summary>
	class Program1
	{
        /// <summary>
        /// The main entry point for the application.
        /// https://msdn.microsoft.com/en-us/magazine/mt620013
        /// </summary>
        [STAThread]
		static void Main()
		{
			Program1 p = new Program1();
			p.Start();
		}

		private readonly Form1 m_mainForm;
		private Program1()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			m_mainForm = new Form1();
		}

		public void Start()
		{
			Application.Run(m_mainForm);
		}
	}
}
