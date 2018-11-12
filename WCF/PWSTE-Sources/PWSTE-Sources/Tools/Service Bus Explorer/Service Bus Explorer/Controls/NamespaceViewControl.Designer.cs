// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

namespace ServiceModelEx.ServiceBus
{
   partial class NamespaceViewControl
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise,false.</param>
      protected override void Dispose(bool disposing)
      {
         if(disposing &&(components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.m_WebBrowser = new System.Windows.Forms.WebBrowser();
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).BeginInit();
         this.SuspendLayout();
         // 
         // m_AddressLabel
         // 
         this.m_AddressLabel.Size = new System.Drawing.Size(228,13);
         this.m_AddressLabel.Text = "sb://<namespace>.servicebus.appfabriclabs.com/...";
         // 
         // m_WebBrowser
         // 
         this.m_WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
         this.m_WebBrowser.Location = new System.Drawing.Point(0,0);
         this.m_WebBrowser.MinimumSize = new System.Drawing.Size(20,20);
         this.m_WebBrowser.Name = "m_WebBrowser";
         this.m_WebBrowser.Size = new System.Drawing.Size(383,335);
         this.m_WebBrowser.TabIndex = 0;
         // 
         // NamespaceViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(this.m_WebBrowser);
         this.Name = "NamespaceViewControl";
         this.Controls.SetChildIndex(this.m_WebBrowser,0);
         this.Controls.SetChildIndex(this.m_ControlAddressCaptionLabel,0);
         this.Controls.SetChildIndex(this.m_CopyButton,0);
         this.Controls.SetChildIndex(this.m_ItemNameLabel,0);
         this.Controls.SetChildIndex(this.m_AddressLabel,0);
         this.Controls.SetChildIndex(this.m_ControlPictureBox,0);
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion

      private System.Windows.Forms.WebBrowser m_WebBrowser;


   }
}
