using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace SolSoft.ObjectOrientedGuiStartup.WindowsForms
{
	/// <summary>
	/// HIDDEN BONUS!
	/// The "host" could also be turned into a class to host arbitrary "programs"
	/// </summary>
	static class FinalMixedDesign
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main()
		{
			//the host environment itself can be made into a class!
			WpfHostEnvironment host = new WpfHostEnvironment();
			host.UnhandledException += (sender, e) =>
			{
				System.Windows.MessageBox.Show(e.ExceptionObject.ToString());
				if(e.IsTerminating)
					host.ShutDown();
			};

			//but it's hard; for example, if you move this constructor before the host constructor
			//the WinForms Application calls will give you grief, because a form will have been constructed already
			//this demonstrates, yet again, why global state is a bad thing
			IAsynchronousHostableProgram program = new FinalMixedDesignProgram();

			return host.Run(program);
		}

		////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Represents a program that can be run
		/// </summary>
		public interface IAsynchronousHostableProgram
		{
			event EventHandler<EventArgs> ExitRequested;
			Task StartAsync();
		}

		////////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Represents a host that uses Wpf to run the UI
		/// If you were so inclined, you could use the "WinFormsAddWpf" to create a "WinForms" host environment
		/// </summary>
		public class WpfHostEnvironment
		{
			private readonly System.Windows.Application m_wpfApplication;

			public WpfHostEnvironment()
			{
				//the WPF application object will be the main UI loop
				m_wpfApplication = new System.Windows.Application
				{
					//otherwise the application will close when all WPF windows are closed
					ShutdownMode = ShutdownMode.OnExplicitShutdown
				};
				SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
				
				//set the WinForms properties
				//notice how you don't need to do anything else with the Windows Forms Application (except handle exceptions)
				System.Windows.Forms.Application.EnableVisualStyles();
				System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

				//wire up exception handling
				System.Windows.Forms.Application.ThreadException += Application_ThreadException;
				m_wpfApplication.DispatcherUnhandledException += m_wpfApplication_DispatcherUnhandledException;
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
				TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
			}

			void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
			{
				OnUnhandledException(new UnhandledExceptionEventArgs(e.Exception, false));
				e.SetObserved();
			}
			void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
			{
				OnUnhandledException(e);
			}
			void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
			{
				OnUnhandledException(new UnhandledExceptionEventArgs(e.Exception, false));
			}
			void m_wpfApplication_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
			{
				OnUnhandledException(new UnhandledExceptionEventArgs(e.Exception, false));
				e.Handled = true;
			}

			public event EventHandler<UnhandledExceptionEventArgs> UnhandledException;
			protected virtual void OnUnhandledException(UnhandledExceptionEventArgs args)
			{
				if (UnhandledException != null)
					UnhandledException(this, args);
			}

			/// <summary>
			/// Runs the application
			/// </summary>
			public int Run(IAsynchronousHostableProgram programToHost)
			{
				try
				{
					programToHost.ExitRequested += programToHost_ExitRequested;

					Task programStart = programToHost.StartAsync();
					HandleExceptions(programStart);

					return m_wpfApplication.Run();
				}
				finally
				{
					//it is good practice to unsubscribe from events on objects that have longer lifetimes than the event handler
					programToHost.ExitRequested -= programToHost_ExitRequested;
				}
			}

			/// <summary>
			/// Runs the task and directs any unhandled exceptions into the global exception handler
			/// </summary>
			/// <param name="task"></param>
			private async void HandleExceptions(Task task)
			{
				try
				{
					await Task.Yield(); //ensure this runs as a continuation
					await task;
				}
				catch (Exception ex)
				{
					OnUnhandledException(new UnhandledExceptionEventArgs(ex, true));
				}
			}

			void programToHost_ExitRequested(object sender, EventArgs e)
			{
				ShutDown();
			}

			/// <summary>
			/// Shuts down the application
			/// </summary>
			public void ShutDown()
			{
				m_wpfApplication.Shutdown();
			}
		}

		////////////////////////////////////////////////////////////////////////


		/// <summary>
		/// Example program to run, same as WpfAddWinForms
		/// </summary>
		public class FinalMixedDesignProgram : IAsynchronousHostableProgram
		{
			private readonly Form1 m_mainForm;
			public FinalMixedDesignProgram()
			{
				m_mainForm = new Form1();
				m_mainForm.FormClosed += m_mainForm_FormClosed;
			}

			public async Task StartAsync()
			{
				SplashScreenWpf splashScreen = new SplashScreenWpf(); //not disposable
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
}
