using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Jira
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private CookieContainer _cookieContainer = new CookieContainer();

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var loginRes = HttpWebRequestExtension.PostWebContent(TrainUrlConstant.AsynSugguestUrl, cookieContainer, "loginUserDTO.user_name=" + userLogin.UserName + "&&userDTO.password=" + userLogin.Password + "&&randCode=" + userLogin.VerifyCode);

            var jiraLoginUrl = @"http://jira.liquid-capital.liquidcap.com/login.jsp";

            var parameters = "os_username=bxu&os_password=simple&os_destination=&user_role=&atl_token=&login=Log+In";
            var loginRes = HttpWebRequestExtension.PostWebContent(jiraLoginUrl, _cookieContainer, parameters);

            var test = _cookieContainer;
        }
    }
}
