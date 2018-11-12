// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

namespace ServiceModelEx.ServiceBus
{
   partial class NodeViewControl
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NodeViewControl));
         this.m_ControlPictureBox = new System.Windows.Forms.PictureBox();
         this.m_CopyButton = new System.Windows.Forms.Button();
         this.m_ControlAddressCaptionLabel = new System.Windows.Forms.Label();
         this.m_AddressLabel = new System.Windows.Forms.Label();
         this.m_ItemNameLabel = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).BeginInit();
         this.SuspendLayout();
         // 
         // m_ControlPictureBox
         // 
         this.m_ControlPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("m_ControlPictureBox.Image")));
         this.m_ControlPictureBox.Location = new System.Drawing.Point(314,273);
         this.m_ControlPictureBox.Name = "m_ControlPictureBox";
         this.m_ControlPictureBox.Size = new System.Drawing.Size(64,60);
         this.m_ControlPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         this.m_ControlPictureBox.TabIndex = 22;
         this.m_ControlPictureBox.TabStop = false;
         this.m_ControlPictureBox.Visible = false;
         // 
         // m_CopyButton
         // 
         this.m_CopyButton.Location = new System.Drawing.Point(10,64);
         this.m_CopyButton.Name = "m_CopyButton";
         this.m_CopyButton.Size = new System.Drawing.Size(92,23);
         this.m_CopyButton.TabIndex = 21;
         this.m_CopyButton.Text = "Copy Address";
         this.m_CopyButton.UseVisualStyleBackColor = true;
         this.m_CopyButton.Visible = false;
         this.m_CopyButton.Click += new System.EventHandler(this.OnCopy);
         // 
         // m_ControlAddressCaptionLabel
         // 
         this.m_ControlAddressCaptionLabel.AutoSize = true;
         this.m_ControlAddressCaptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
         this.m_ControlAddressCaptionLabel.Location = new System.Drawing.Point(7,32);
         this.m_ControlAddressCaptionLabel.Name = "m_ControlAddressCaptionLabel";
         this.m_ControlAddressCaptionLabel.Size = new System.Drawing.Size(100,13);
         this.m_ControlAddressCaptionLabel.TabIndex = 19;
         this.m_ControlAddressCaptionLabel.Text = "MyItem Address:";
         this.m_ControlAddressCaptionLabel.Visible = false;
         // 
         // m_AddressLabel
         // 
         this.m_AddressLabel.AutoSize = true;
         this.m_AddressLabel.Location = new System.Drawing.Point(8,45);
         this.m_AddressLabel.Name = "m_AddressLabel";
         this.m_AddressLabel.Size = new System.Drawing.Size(209,13);
         this.m_AddressLabel.TabIndex = 20;
         this.m_AddressLabel.Text = "sb://<namespace>.servicebus.appfabriclabs.com/...";
         this.m_AddressLabel.Visible = false;
         // 
         // m_ItemNameLabel
         // 
         this.m_ItemNameLabel.AutoSize = true;
         this.m_ItemNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",14F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
         this.m_ItemNameLabel.Location = new System.Drawing.Point(6,3);
         this.m_ItemNameLabel.Name = "m_ItemNameLabel";
         this.m_ItemNameLabel.Size = new System.Drawing.Size(76,24);
         this.m_ItemNameLabel.TabIndex = 18;
         this.m_ItemNameLabel.Text = "MyItem";
         this.m_ItemNameLabel.Visible = false;
         // 
         // NodeViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ControlDark;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.m_ControlPictureBox);
         this.Controls.Add(this.m_AddressLabel);
         this.Controls.Add(this.m_ItemNameLabel);
         this.Controls.Add(this.m_CopyButton);
         this.Controls.Add(this.m_ControlAddressCaptionLabel);
         this.Name = "NodeViewControl";
         this.Size = new System.Drawing.Size(385,337);
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      protected System.Windows.Forms.Label m_AddressLabel;
      protected System.Windows.Forms.Label m_ItemNameLabel;
      protected System.Windows.Forms.PictureBox m_ControlPictureBox;
      protected System.Windows.Forms.Label m_ControlAddressCaptionLabel;
      protected System.Windows.Forms.Button m_CopyButton;
   }
}
