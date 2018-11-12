using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WPF_Sleep_Delay
{
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/vstudio/hh156528.aspx
    /// https://social.technet.microsoft.com/wiki/contents/articles/21177.visual-c-thread-sleep-vs-task-delay.aspx
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "The UI thread is not blocked during the delay.";
            // Call the method that runs asynchronously.
            string result = await WaitAsynchronouslyAsync();

            // Call the method that runs synchronously.
            //string result = await WaitSynchronously ();

            // Display the result.
            textBox1.Text = result;
        }

        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "UI thread is blocked.";

            // Call the method that runs synchronously.
            string result = await WaitSynchronously();

            // Display the result.
            textBox1.Text = result;
        }

        // The following method runs asynchronously. The UI thread is not
        // blocked during the delay. You can move or resize the Form1 window 
        // while Task.Delay is running.
        public async Task<string> WaitAsynchronouslyAsync()
        {
            await Task.Delay(10000);
            return "WaitAsynchronouslyAsync Finished";
        }

        // The following method runs synchronously, despite the use of async.
        // You cannot move or resize the Form1 window while Thread.Sleep
        // is running because the UI thread is blocked.
        public async Task<string> WaitSynchronously()
        {
            // Add a using directive for System.Threading.
            Thread.Sleep(10000);
            return "WaitSynchronously Finished";
        }
    }
}
