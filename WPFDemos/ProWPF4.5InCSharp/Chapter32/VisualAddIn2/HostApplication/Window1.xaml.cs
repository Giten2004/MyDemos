using System;
using System.Windows;
using System.Windows.Controls;
using System.AddIn.Hosting;
using HostView;
using System.Collections.ObjectModel;

namespace ApplicationHost
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get add-in pipeline folder (the folder in which this application was launched from)
            string appPath = System.IO.Path.Combine(Environment.CurrentDirectory, "AddIns");

            //update must use the host root folder
            AddInStore.Update(Environment.CurrentDirectory);

            if (System.IO.Directory.Exists(appPath))
            {
                // Rebuild visual add-in pipeline
                string[] warnings = AddInStore.RebuildAddIns(appPath);
                if (warnings.Length > 0)
                {
                    string msg = "Could not rebuild pipeline:";
                    foreach (string warning in warnings)
                        msg += "\n" + warning;
                    MessageBox.Show(msg);
                    return;
                }
            }

            // Activate add-in with Internet zone security isolation
            Collection<AddInToken> addInTokens = AddInStore.FindAddIns(typeof(IWPFAddInHostView), PipelineStoreLocation.ApplicationBase, new string[] { appPath });
            lstAddIns.ItemsSource = addInTokens;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddInToken wpfAddInToken = (AddInToken)lstAddIns.SelectedItem;
            var wpfAddInHostView = wpfAddInToken.Activate<IWPFAddInHostView>(AddInSecurityLevel.FullTrust);

            // Get and display add-in UI
            FrameworkElement addInUI = wpfAddInHostView.GetAddInUI();

            var tabItem = new TabItem();
            tabItem.Content = addInUI;
            pluginTab.Items.Add(tabItem);
            pluginTab.SelectedIndex = pluginTab.Items.Count - 1;

            //pnlAddIn.Child = addInUI;
        }

        private void lstAddIns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOpenPlugin.IsEnabled = lstAddIns.SelectedIndex != -1;
        }
    }
}
