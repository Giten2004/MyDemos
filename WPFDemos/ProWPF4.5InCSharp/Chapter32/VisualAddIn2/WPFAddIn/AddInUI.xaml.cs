using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFAddIn
{
    /// <summary>
    /// Interaction logic for AddInUI.xaml
    /// </summary>
    public partial class AddInUI : UserControl
    {
        public AddInUI()
        {
            InitializeComponent();
        }

        private void clickMeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello from WPFAddIn1");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newWin = new Window1();
            newWin.Show();
        }
    }
}
