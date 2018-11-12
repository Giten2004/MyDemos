using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiduWaiMai
{
    public partial class Form1 : Form
    {
        private int _flag = 0;
        private string[,] _user = new string[1000, 3];
        private int _userCount = 0;
        private int _userTotal = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //int count = 0;
            //StreamReader sr = File.OpenText("user.txt");
            //string temp = sr.ReadLine();
            //while (temp != null)
            //{
            //    string[] temps = temp.Split(' ');
            //    _user[count, 0] = temps[0];
            //    _user[count, 1] = temps[1];
            //    count++;
            //    temp = sr.ReadLine();
            //}
            //_userTotal = count;

            _userTotal = 2;
            _user[0, 0] = "398755692@qq.com";
            _user[0, 1] = "输入密码";
            _user[0, 2] = "18683254780";

            _user[1, 0] = "username2";
            _user[1, 1] = "username2_pwd";
            _user[1, 1] = "18683254780";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _flag = 1;
            webBrowser1.Navigate("https://passport.baidu.com");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (_flag == 1)
            {
                _flag = 2;

                if (_userCount == _userTotal)
                {
                    MessageBox.Show("已经到达最后一个！");
                    return;
                }

                HtmlDocument doc = webBrowser1.Document;

                HtmlElement userName = doc.GetElementById("userName");
                userName.InnerText = _user[_userCount, 0];

                HtmlElement password = doc.GetElementById("password");
                password.InnerText = _user[_userCount, 1];

                HtmlElement memberPass = doc.GetElementById("memberPass");
                memberPass.SetAttribute("checked", "");

                HtmlElement submit = doc.GetElementById("TANGRAM__PSP_3__submit");
                submit.InvokeMember("click");

                label2.Text = _user[_userCount, 0];
                label3.Text = _user[_userCount, 1];
                label4.Text = _user[_userCount, 2];

                _userCount++;

                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = label3.Text = "空";
            webBrowser1.Navigate("https://passport.baidu.com/?logout");
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                webBrowser1.Navigate("http://waimai.baidu.com/waimai/shop/1494591864233231885");
            }

            if (radioButton2.Checked == true)
            {
                webBrowser1.Navigate("http://waimai.baidu.com/waimai/shop/17074006309063905116");
            }

            if (radioButton3.Checked == true)
            {
                webBrowser1.Navigate("https://waimai.baidu.com/waimai/shop/1505854901");
            }

            webBrowser1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = webBrowser1.Document;

            HtmlElement searchCon = doc.GetElementById("cartSubmit");
            searchCon.InvokeMember("click");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = webBrowser1.Document;

            HtmlElement searchCon = doc.GetElementById("orderSubmit");
            searchCon.InvokeMember("click");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = webBrowser1.Document;
            HtmlElement userName = doc.GetElementById("user_name");
            userName.SetAttribute("value", "白冰");

            HtmlElement userPhone = doc.GetElementById("user_phone");
            userPhone.SetAttribute("value", label4.Text);

            HtmlElement userAdd = doc.GetElementById("sug_address");
            userAdd.SetAttribute("value", "勒泰");

            HtmlElement userDet = doc.GetElementById("detail_address");
            userDet.SetAttribute("value", "勒泰写字楼B座37层");

            HtmlElementCollection htmlele = webBrowser1.Document.GetElementsByTagName("input");
            foreach (HtmlElement item in htmlele)
            {
                if (item.OuterHtml == "<input class=\"saveBtn\" type=\"button\" value=\"保存送餐信息\" data-node=\"saveBtn\">")
                {
                    item.InvokeMember("click");
                }
            }

            HtmlElement userMark = doc.GetElementById("mark");
            userMark.SetAttribute("value", "5784");
        }
    }
}