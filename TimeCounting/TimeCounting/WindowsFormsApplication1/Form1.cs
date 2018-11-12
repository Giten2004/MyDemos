using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MLTimerDot.MLTimer timer = new MLTimerDot.MLTimer();
            timer.Start();
            Thread.Sleep(1000);
            UInt64 cpuspeed10 = (ulong)(timer.Stop() / 100000); //通过这个可以算出 CPU 的mhz

            timer.Start();
            //测试代码（测试声明一个DataTable 用的时间）
            DataTable td = new DataTable();

            UInt64 time1 = timer.Stop();
            String s = String.Format("CPU {0}.{1} mhz \n声明 MLTimer 类的系统开销 {2:n} 时钟周期 \n本操作系统开销 {3:n} 个时钟周期 \n使用 {4:n} ns",
                    cpuspeed10 / 10, cpuspeed10 % 10, timer.get_Overhead(),
                    time1,
                    time1 * 10000 / cpuspeed10);

            MessageBox.Show(s);
        }
    }
}
