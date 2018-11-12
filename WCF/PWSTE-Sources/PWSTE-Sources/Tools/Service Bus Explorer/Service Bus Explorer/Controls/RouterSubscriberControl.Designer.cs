// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

namespace ServiceModelEx.ServiceBus
{
   partial class RouterSubscriberViewControl
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RouterSubscriberViewControl));
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).BeginInit();
         this.SuspendLayout();
         // 
         // m_AddressLabel
         // 
         this.m_AddressLabel.Visible = true;
         // 
         // m_ControlTitleLabel
         // 
         this.m_ItemNameLabel.Size = new System.Drawing.Size(144,24);
         this.m_ItemNameLabel.Text = "My Subscriber";
         this.m_ItemNameLabel.Visible = true;
         // 
         // m_ControlPictureBox
         // 
         this.m_ControlPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("m_ControlPictureBox.Image")));
         this.m_ControlPictureBox.Location = new System.Drawing.Point(177,235);
         this.m_ControlPictureBox.Size = new System.Drawing.Size(193,88);
         this.m_ControlPictureBox.Visible = true;
         // 
         // m_ControlTitleCaptionLabel
         // 
         this.m_ControlAddressCaptionLabel.Size = new System.Drawing.Size(120,13);
         this.m_ControlAddressCaptionLabel.Text = "Subscriber Address:";
         this.m_ControlAddressCaptionLabel.Visible = true;
         // 
         // m_CopyButton
         // 
         this.m_CopyButton.Visible = true;
         // 
         // RouterSubscriberViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Name = "RouterSubscriberViewControl";
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion
   }
}
