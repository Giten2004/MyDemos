using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolSoft.ObjectOrientedGuiStartup.Wpf
{
	class MainViewModel
	{
		public event EventHandler<EventArgs> CloseRequested;
		public void RequestClose()
		{
			OnCloseRequested(EventArgs.Empty);
		}

		protected virtual void OnCloseRequested(EventArgs e)
		{
			if (CloseRequested != null)
				CloseRequested(this, e);
		}

		public void Initialize()
		{
			Thread.Sleep(TimeSpan.FromSeconds(5));
		}


		public Task InitializeAsync()
		{
			//use this to test the exception handling
			//throw new NotImplementedException();
			return Task.Delay(TimeSpan.FromSeconds(5));
		}

		
	}
}
