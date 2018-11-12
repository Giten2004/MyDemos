// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

namespace ServiceModelEx.ServiceBus
{
   partial class NewBufferDialog
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if(disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.Windows.Forms.Label addressLabel;
         System.Windows.Forms.GroupBox bufferGroupBox;
         System.Windows.Forms.Label renewLabel;
         System.Windows.Forms.Label overflowLabel;
         System.Windows.Forms.Label countLabel;
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewBufferDialog));
         this.m_CreateButton = new System.Windows.Forms.Button();
         this.m_AddressTextBox = new System.Windows.Forms.TextBox();
         this.m_ExpirationTime = new System.Windows.Forms.TextBox();
         this.m_OverflowComboBox = new System.Windows.Forms.ComboBox();
         this.m_CountTextBox = new System.Windows.Forms.TextBox();
         addressLabel = new System.Windows.Forms.Label();
         bufferGroupBox = new System.Windows.Forms.GroupBox();
         renewLabel = new System.Windows.Forms.Label();
         overflowLabel = new System.Windows.Forms.Label();
         countLabel = new System.Windows.Forms.Label();
         bufferGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // addressLabel
         // 
         addressLabel.AutoSize = true;
         addressLabel.Location = new System.Drawing.Point(5,9);
         addressLabel.Name = "addressLabel";
         addressLabel.Size = new System.Drawing.Size(83,13);
         addressLabel.TabIndex = 0;
         addressLabel.Text = "Buffer Address:";
         // 
         // m_CreateButton
         // 
         this.m_CreateButton.Enabled = false;
         this.m_CreateButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
         this.m_CreateButton.Location = new System.Drawing.Point(291,158);
         this.m_CreateButton.Name = "m_CreateButton";
         this.m_CreateButton.Size = new System.Drawing.Size(78,23);
         this.m_CreateButton.TabIndex = 7;
         this.m_CreateButton.Text = "Create";
         this.m_CreateButton.UseVisualStyleBackColor = true;
         this.m_CreateButton.Click += new System.EventHandler(this.OnCreate);
         // 
         // m_AddressTextBox
         // 
         this.m_AddressTextBox.Location = new System.Drawing.Point(7,25);
         this.m_AddressTextBox.Name = "m_AddressTextBox";
         this.m_AddressTextBox.Size = new System.Drawing.Size(352,20);
         this.m_AddressTextBox.TabIndex = 1;
         this.m_AddressTextBox.TextChanged += new System.EventHandler(this.OnTextChanged);
         // 
         // bufferGroupBox
         // 
         bufferGroupBox.Controls.Add(this.m_ExpirationTime);
         bufferGroupBox.Controls.Add(renewLabel);
         bufferGroupBox.Controls.Add(overflowLabel);
         bufferGroupBox.Controls.Add(this.m_OverflowComboBox);
         bufferGroupBox.Controls.Add(countLabel);
         bufferGroupBox.Controls.Add(this.m_CountTextBox);
         bufferGroupBox.Location = new System.Drawing.Point(8,63);
         bufferGroupBox.Name = "bufferGroupBox";
         bufferGroupBox.Size = new System.Drawing.Size(361,75);
         bufferGroupBox.TabIndex = 15;
         bufferGroupBox.TabStop = false;
         bufferGroupBox.Text = "Buffer";
         // 
         // m_ExpirationTime
         // 
         this.m_ExpirationTime.Location = new System.Drawing.Point(238,32);
         this.m_ExpirationTime.Name = "m_ExpirationTime";
         this.m_ExpirationTime.Size = new System.Drawing.Size(113,20);
         this.m_ExpirationTime.TabIndex = 19;
         this.m_ExpirationTime.Text = "5";
         // 
         // renewLabel
         // 
         renewLabel.AutoSize = true;
         renewLabel.Location = new System.Drawing.Point(235,15);
         renewLabel.Name = "renewLabel";
         renewLabel.Size = new System.Drawing.Size(101,13);
         renewLabel.TabIndex = 18;
         renewLabel.Text = "Expiration (minutes):";
         // 
         // overflowLabel
         // 
         overflowLabel.AutoSize = true;
         overflowLabel.Location = new System.Drawing.Point(118,16);
         overflowLabel.Name = "overflowLabel";
         overflowLabel.Size = new System.Drawing.Size(52,13);
         overflowLabel.TabIndex = 14;
         overflowLabel.Text = "Overflow:";
         // 
         // m_OverflowComboBox
         // 
         this.m_OverflowComboBox.FormattingEnabled = true;
         this.m_OverflowComboBox.Items.AddRange(new object[] {
            "Reject"});
         this.m_OverflowComboBox.Location = new System.Drawing.Point(121,32);
         this.m_OverflowComboBox.Name = "m_OverflowComboBox";
         this.m_OverflowComboBox.Size = new System.Drawing.Size(106,21);
         this.m_OverflowComboBox.TabIndex = 13;
         // 
         // countLabel
         // 
         countLabel.AutoSize = true;
         countLabel.Location = new System.Drawing.Point(6,16);
         countLabel.Name = "countLabel";
         countLabel.Size = new System.Drawing.Size(38,13);
         countLabel.TabIndex = 0;
         countLabel.Text = "Count:";
         // 
         // m_CountTextBox
         // 
         this.m_CountTextBox.Location = new System.Drawing.Point(9,32);
         this.m_CountTextBox.Name = "m_CountTextBox";
         this.m_CountTextBox.Size = new System.Drawing.Size(103,20);
         this.m_CountTextBox.TabIndex = 1;
         this.m_CountTextBox.Text = "10";
         // 
         // NewBufferDialog
         // 
         this.AcceptButton = this.m_CreateButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(377,188);
         this.Controls.Add(bufferGroupBox);
         this.Controls.Add(this.m_CreateButton);
         this.Controls.Add(this.m_AddressTextBox);
         this.Controls.Add(addressLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "NewBufferDialog";
         this.Text = "Create New Buffer";
         bufferGroupBox.ResumeLayout(false);
         bufferGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button m_CreateButton;
      private System.Windows.Forms.TextBox m_AddressTextBox;
      private System.Windows.Forms.TextBox m_ExpirationTime;
      private System.Windows.Forms.ComboBox m_OverflowComboBox;
      private System.Windows.Forms.TextBox m_CountTextBox;

   }
}