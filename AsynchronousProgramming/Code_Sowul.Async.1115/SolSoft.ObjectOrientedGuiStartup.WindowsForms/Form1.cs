using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			Font = SystemFonts.MessageBoxFont;
			InitializeComponent();
		}

		public Task InitializeAsync()
		{
			//use this to test the exception handling
			//throw new NotImplementedException();
			return Task.Delay(TimeSpan.FromSeconds(5));
		}

		public void Initialize()
		{
			Thread.Sleep(TimeSpan.FromSeconds(5));
		}
	}
}
