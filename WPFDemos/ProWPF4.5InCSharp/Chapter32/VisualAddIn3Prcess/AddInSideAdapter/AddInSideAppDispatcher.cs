using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AddInSideAdapter
{
    internal class AddInSideAppDispatcher
    {
        private System.Windows.Application _app;
        private String eventName = "appStarted:";
        private readonly ManualResetEventSlim _appStarted;

        private Thread _myThread;

        private readonly object _locker = new object();

        public AddInSideAppDispatcher()
        {
            MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
            Debug.WriteLine(string.Format("calling constructor AddInSideAppDispatcher, TheadId:{0}", Thread.CurrentThread.ManagedThreadId));
            _appStarted = new ManualResetEventSlim(false);

            Thread t = new Thread(new ThreadStart(STAThreadWorker));
            t.SetApartmentState(ApartmentState.STA);
            t.Name = "AddInSideAdapters.AppDispatcher";
            t.Start();

            _appStarted.Wait();
        }

        private void STAThreadWorker()
        {
            MessageBox.Show("STAThreadWorker" + Thread.CurrentThread.ManagedThreadId.ToString());
            Debug.WriteLine(string.Format("STAThreadWorker, TheadId:{0}", Thread.CurrentThread.ManagedThreadId));

            if (System.Windows.Application.Current == null)
            {
                lock (_locker)
                {
                    _app = new System.Windows.Application();
                    _app.DispatcherUnhandledException += app_DispatcherUnhandledException;
                    _app.SessionEnding += _app_SessionEnding;

                    //This is very important
                    _app.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                    _app.Startup += _app_Startup;
                }
                _app.Run();
            }
            else
            {
                lock (_locker)
                {
                    _app = System.Windows.Application.Current;

                    _appStarted.Set();
                }
            }
        }

        private void _app_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            Debug.WriteLine("AddInSideAppDispatcher is ending");
        }

        private void _app_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            Debug.WriteLine("AddInSideAppDispatcher is starting");
            _appStarted.Set();
        }

        // Certain exceptions can occur in UI stack traces, and this is the only place to handle them,
        // if possible, and avoid a total crash.
        private void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("AddInSideAppDispatcher UnhandledException");
            var comException = e.Exception as System.Runtime.InteropServices.COMException;

            // Solution for clipboard crash taken from here: http://stackoverflow.com/a/13523188            
            if (comException != null && comException.ErrorCode == -2147221040)
            {
                e.Handled = true;
                // In my testing, even though this error is thrown, the data is still copied to the clipboard.
                // Thus instead of warning the user, the code will ignore the error so that the user sees 
                // everything as working correctly.
                //app.Dispatcher.BeginInvoke((Action)DisplayClipboardError);
                return;
            }
        }

        public void DoWork(Worker d)
        {
            MessageBox.Show("DoWork" + Thread.CurrentThread.ManagedThreadId.ToString());
            Debug.WriteLine(string.Format("calling DoWork, TheadId:{0}", Thread.CurrentThread.ManagedThreadId));
            lock (_locker)
            {
                if (!Thread.CurrentThread.Equals(_myThread))
                {
                    _app.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, d);
                }
                else
                {
                    d.Invoke();
                }
            }
        }

        public object DoWork(Func<object> d)
        {
            Debug.WriteLine(string.Format("calling DoWork, TheadId:{0}", Thread.CurrentThread.ManagedThreadId));
            lock (_locker)
            {
                if (!Thread.CurrentThread.Equals(_myThread))
                {
                    return _app.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, d);
                }
                else
                {
                    return d.Invoke();
                }
            }
        }
    }

    internal delegate void Worker();
}
