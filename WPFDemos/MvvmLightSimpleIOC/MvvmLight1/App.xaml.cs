using System.Windows;
using GalaSoft.MvvmLight.Threading;
using MvvmLight1.Model;
using MvvmLight1.ViewModel;

namespace MvvmLight1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProxy _proxy;
        public App()
        {
            DispatcherHelper.Initialize();
            _proxy = new ProxyImplement();

            VMLocator = new ViewModelLocator();
            VMLocator.RegisterInstance(_proxy);
            VMLocator.RegisterInstance(new ProxyImplement());
        }

        public static ViewModelLocator VMLocator { get; private set; }
    }
}
