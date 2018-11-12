// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace WinFormsEx
{
   internal class SplashForm : Form
   {
      System.Windows.Forms.Timer m_Timer;
      PictureBox m_SplashPictureBox;
      private System.ComponentModel.IContainer components;
      bool m_HideSplash = false;
      
      public SplashForm(Bitmap splashImage)
      {
         InitializeComponent();
         m_SplashPictureBox.Image = splashImage;
         ClientSize = m_SplashPictureBox.Size;
      }
      #region Windows Form Designer generated code
      void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.m_SplashPictureBox = new System.Windows.Forms.PictureBox();
         this.m_Timer = new System.Windows.Forms.Timer(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.m_SplashPictureBox)).BeginInit();
         this.SuspendLayout();
         // 
         // m_SplashPictureBox
         // 
         this.m_SplashPictureBox.Cursor = System.Windows.Forms.Cursors.AppStarting;
         this.m_SplashPictureBox.Image = global::ServiceModelEx.Properties.Resources.Progress;
         this.m_SplashPictureBox.Location = new System.Drawing.Point(0,0);
         this.m_SplashPictureBox.Name = "m_SplashPictureBox";
         this.m_SplashPictureBox.Size = new System.Drawing.Size(206,42);
         this.m_SplashPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.m_SplashPictureBox.TabIndex = 0;
         this.m_SplashPictureBox.TabStop = false;
         // 
         // m_Timer
         // 
         this.m_Timer.Enabled = true;
         this.m_Timer.Interval = 500;
         this.m_Timer.Tick += new System.EventHandler(this.OnTick);
         // 
         // SplashForm
         // 
         this.ClientSize = new System.Drawing.Size(205,19);
         this.ControlBox = false;
         this.Controls.Add(this.m_SplashPictureBox);
         this.Cursor = System.Windows.Forms.Cursors.Cross;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Name = "SplashForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.TopMost = true;
         ((System.ComponentModel.ISupportInitialize)(this.m_SplashPictureBox)).EndInit();
         this.ResumeLayout(false);

      }
      #endregion
      void OnTick(object sender,EventArgs e)
      {
         if(HideSplash == true)
         {
            m_Timer.Enabled = false;
            Close();
         }
      }
      public bool HideSplash
      {
         get
         {
            lock(this)
            {
               return m_HideSplash;
            }
         }
         set
         {
            lock(this)
            {
               m_HideSplash = value;   
            }
         }
      }
   }
   public class SplashScreen
   {
      Point m_Location;
      Bitmap m_SplashImage;
      SplashForm m_SplashForm;
      Thread m_WorkerThread;
      ManualResetEvent m_FromCreated = new ManualResetEvent(false);

      public SplashScreen(Bitmap splash)
      {
         Form parent = Application.OpenForms[0];
         int y = (parent.Height - splash.Size.Height)/2;
         int x = (parent.Width - splash.Size.Width)/2;
         m_Location = new Point(parent.Location.X + x,parent.Location.Y + y);
         m_SplashImage = splash;
         m_WorkerThread = new Thread(Show);
         m_WorkerThread.IsBackground = true;
         m_WorkerThread.Start();
         m_FromCreated.WaitOne();
      }
      void Show()
      {
         m_SplashForm = new SplashForm(m_SplashImage);
         m_SplashForm.Location = m_Location;
         m_FromCreated.Set();
         m_SplashForm.ShowDialog();
      }
      public void Close()
      {
         m_SplashForm.HideSplash = true;
         m_WorkerThread.Join();
      }
   }
 }