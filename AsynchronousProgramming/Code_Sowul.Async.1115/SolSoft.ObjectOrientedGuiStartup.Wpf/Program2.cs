using System;
using System.Windows;

namespace SolSoft.ObjectOrientedGuiStartup.Wpf
{
    /// <summary>
    ///     Second refactoring: separate message loop
    /// </summary>
    internal class Program2
    {
        private readonly AppNoStartupUri m_app;

        private Program2()
        {
            m_app = new AppNoStartupUri
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown
            };
            m_app.InitializeComponent();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var p = new Program2();
            p.Start();
        }

        public void Start()
        {
            var viewModel = new MainViewModel();
            viewModel.CloseRequested += viewModel_CloseRequested;
            viewModel.Initialize();

            var mainForm = new MainWindow();
            mainForm.Closed += (sender, e) => { viewModel.RequestClose(); };

            mainForm.DataContext = viewModel;
            mainForm.Show();
            m_app.Run();
        }

        private void viewModel_CloseRequested(object sender, EventArgs e)
        {
            m_app.Shutdown();
        }
    }
}