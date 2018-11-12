using System;

namespace SolSoft.ObjectOrientedGuiStartup.Wpf
{
    /// <summary>
    ///     First refactoring: closer to object-oriented
    /// </summary>
    internal class Program1
    {
        private readonly App m_app;

        private Program1()
        {
            m_app = new App();

            //this applies the XAML, e.g. StartupUri, Application.Resources
            m_app.InitializeComponent();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var p = new Program1();
            p.Start();
        }

        public void Start()
        {
            //shows the Window specified by StartupUri
            m_app.Run();
        }
    }
}