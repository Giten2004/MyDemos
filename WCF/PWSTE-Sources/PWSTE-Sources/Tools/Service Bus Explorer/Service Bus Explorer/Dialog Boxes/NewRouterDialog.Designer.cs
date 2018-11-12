// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

namespace ServiceModelEx.ServiceBus
{
   partial class NewRouterDialog
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
         System.Windows.Forms.GroupBox routerGroupBox;
         System.Windows.Forms.Label renewLabel;
         System.Windows.Forms.Label pushLabel;
         System.Windows.Forms.Label overflowLabel;
         System.Windows.Forms.Label bufferLengthLabel;
         System.Windows.Forms.GroupBox subscribersGroupBox;
         System.Windows.Forms.Label maxSubscribersLabel;
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRouterDialog));
         this.m_ExpirationTimePicker = new System.Windows.Forms.DateTimePicker();
         this.m_PushRetriesTextBox = new System.Windows.Forms.TextBox();
         this.m_OverflowComboBox = new System.Windows.Forms.ComboBox();
         this.m_BufferLengthTextBox = new System.Windows.Forms.TextBox();
         this.m_OneRadioButton = new System.Windows.Forms.RadioButton();
         this.m_AllRadioButton = new System.Windows.Forms.RadioButton();
         this.m_MaxSubscribersTextBox = new System.Windows.Forms.TextBox();
         this.m_CreateButton = new System.Windows.Forms.Button();
         this.m_AddressTextBox = new System.Windows.Forms.TextBox();
         addressLabel = new System.Windows.Forms.Label();
         routerGroupBox = new System.Windows.Forms.GroupBox();
         renewLabel = new System.Windows.Forms.Label();
         pushLabel = new System.Windows.Forms.Label();
         overflowLabel = new System.Windows.Forms.Label();
         bufferLengthLabel = new System.Windows.Forms.Label();
         subscribersGroupBox = new System.Windows.Forms.GroupBox();
         maxSubscribersLabel = new System.Windows.Forms.Label();
         routerGroupBox.SuspendLayout();
         subscribersGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // addressLabel
         // 
         addressLabel.AutoSize = true;
         addressLabel.Location = new System.Drawing.Point(5,9);
         addressLabel.Name = "addressLabel";
         addressLabel.Size = new System.Drawing.Size(83,13);
         addressLabel.TabIndex = 0;
         addressLabel.Text = "Router Address:";
         // 
         // routerGroupBox
         // 
         routerGroupBox.Controls.Add(this.m_ExpirationTimePicker);
         routerGroupBox.Controls.Add(renewLabel);
         routerGroupBox.Controls.Add(pushLabel);
         routerGroupBox.Controls.Add(this.m_PushRetriesTextBox);
         routerGroupBox.Controls.Add(overflowLabel);
         routerGroupBox.Controls.Add(this.m_OverflowComboBox);
         routerGroupBox.Controls.Add(bufferLengthLabel);
         routerGroupBox.Controls.Add(this.m_BufferLengthTextBox);
         routerGroupBox.Location = new System.Drawing.Point(134,72);
         routerGroupBox.Name = "routerGroupBox";
         routerGroupBox.Size = new System.Drawing.Size(235,125);
         routerGroupBox.TabIndex = 14;
         routerGroupBox.TabStop = false;
         routerGroupBox.Text = "Router";
         // 
         // m_ExpirationTimePicker
         // 
         this.m_ExpirationTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
         this.m_ExpirationTimePicker.Location = new System.Drawing.Point(123,42);
         this.m_ExpirationTimePicker.Name = "m_ExpirationTimePicker";
         this.m_ExpirationTimePicker.Size = new System.Drawing.Size(106,20);
         this.m_ExpirationTimePicker.TabIndex = 19;
         // 
         // renewLabel
         // 
         renewLabel.AutoSize = true;
         renewLabel.Location = new System.Drawing.Point(120,25);
         renewLabel.Name = "renewLabel";
         renewLabel.Size = new System.Drawing.Size(56,13);
         renewLabel.TabIndex = 18;
         renewLabel.Text = "Expiration:";
         // 
         // pushLabel
         // 
         pushLabel.AutoSize = true;
         pushLabel.Location = new System.Drawing.Point(8,25);
         pushLabel.Name = "pushLabel";
         pushLabel.Size = new System.Drawing.Size(84,13);
         pushLabel.TabIndex = 15;
         pushLabel.Text = "Delivery Retries:";
         // 
         // m_PushRetriesTextBox
         // 
         this.m_PushRetriesTextBox.Location = new System.Drawing.Point(11,41);
         this.m_PushRetriesTextBox.Name = "m_PushRetriesTextBox";
         this.m_PushRetriesTextBox.Size = new System.Drawing.Size(103,20);
         this.m_PushRetriesTextBox.TabIndex = 16;
         // 
         // overflowLabel
         // 
         overflowLabel.AutoSize = true;
         overflowLabel.Location = new System.Drawing.Point(120,73);
         overflowLabel.Name = "overflowLabel";
         overflowLabel.Size = new System.Drawing.Size(83,13);
         overflowLabel.TabIndex = 14;
         overflowLabel.Text = "Overflow Policy:";
         // 
         // m_OverflowComboBox
         // 
         this.m_OverflowComboBox.FormattingEnabled = true;
         this.m_OverflowComboBox.Items.AddRange(new object[] {
            "Reject",
            "Discard Incoming",
            "Discard Existing"});
         this.m_OverflowComboBox.Location = new System.Drawing.Point(123,89);
         this.m_OverflowComboBox.Name = "m_OverflowComboBox";
         this.m_OverflowComboBox.Size = new System.Drawing.Size(106,21);
         this.m_OverflowComboBox.TabIndex = 13;
         // 
         // bufferLengthLabel
         // 
         bufferLengthLabel.AutoSize = true;
         bufferLengthLabel.Location = new System.Drawing.Point(8,73);
         bufferLengthLabel.Name = "bufferLengthLabel";
         bufferLengthLabel.Size = new System.Drawing.Size(70,13);
         bufferLengthLabel.TabIndex = 0;
         bufferLengthLabel.Text = "Buffer length:";
         // 
         // m_BufferLengthTextBox
         // 
         this.m_BufferLengthTextBox.Location = new System.Drawing.Point(11,89);
         this.m_BufferLengthTextBox.Name = "m_BufferLengthTextBox";
         this.m_BufferLengthTextBox.Size = new System.Drawing.Size(103,20);
         this.m_BufferLengthTextBox.TabIndex = 1;
         // 
         // subscribersGroupBox
         // 
         subscribersGroupBox.Controls.Add(this.m_OneRadioButton);
         subscribersGroupBox.Controls.Add(this.m_AllRadioButton);
         subscribersGroupBox.Controls.Add(this.m_MaxSubscribersTextBox);
         subscribersGroupBox.Controls.Add(maxSubscribersLabel);
         subscribersGroupBox.Location = new System.Drawing.Point(7,72);
         subscribersGroupBox.Name = "subscribersGroupBox";
         subscribersGroupBox.Size = new System.Drawing.Size(115,125);
         subscribersGroupBox.TabIndex = 13;
         subscribersGroupBox.TabStop = false;
         subscribersGroupBox.Text = "Subscribers";
         // 
         // m_OneRadioButton
         // 
         this.m_OneRadioButton.AutoSize = true;
         this.m_OneRadioButton.Location = new System.Drawing.Point(6,42);
         this.m_OneRadioButton.Name = "m_OneRadioButton";
         this.m_OneRadioButton.Size = new System.Drawing.Size(98,17);
         this.m_OneRadioButton.TabIndex = 1;
         this.m_OneRadioButton.TabStop = true;
         this.m_OneRadioButton.Text = "One Subscriber";
         this.m_OneRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_AllRadioButton
         // 
         this.m_AllRadioButton.AutoSize = true;
         this.m_AllRadioButton.Location = new System.Drawing.Point(6,19);
         this.m_AllRadioButton.Name = "m_AllRadioButton";
         this.m_AllRadioButton.Size = new System.Drawing.Size(94,17);
         this.m_AllRadioButton.TabIndex = 0;
         this.m_AllRadioButton.TabStop = true;
         this.m_AllRadioButton.Text = "All Subscribers";
         this.m_AllRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_MaxSubscribersTextBox
         // 
         this.m_MaxSubscribersTextBox.Location = new System.Drawing.Point(8,89);
         this.m_MaxSubscribersTextBox.Name = "m_MaxSubscribersTextBox";
         this.m_MaxSubscribersTextBox.Size = new System.Drawing.Size(96,20);
         this.m_MaxSubscribersTextBox.TabIndex = 3;
         // 
         // maxSubscribersLabel
         // 
         maxSubscribersLabel.AutoSize = true;
         maxSubscribersLabel.Location = new System.Drawing.Point(6,72);
         maxSubscribersLabel.Name = "maxSubscribersLabel";
         maxSubscribersLabel.Size = new System.Drawing.Size(88,13);
         maxSubscribersLabel.TabIndex = 2;
         maxSubscribersLabel.Text = "Max Subscribers:";
         // 
         // m_CreateButton
         // 
         this.m_CreateButton.Enabled = false;
         this.m_CreateButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
         this.m_CreateButton.Location = new System.Drawing.Point(291,224);
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
         // NewRouterDialog
         // 
         this.AcceptButton = this.m_CreateButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(377,258);
         this.Controls.Add(routerGroupBox);
         this.Controls.Add(subscribersGroupBox);
         this.Controls.Add(this.m_CreateButton);
         this.Controls.Add(this.m_AddressTextBox);
         this.Controls.Add(addressLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "NewRouterDialog";
         this.Text = "Create New Router";
         routerGroupBox.ResumeLayout(false);
         routerGroupBox.PerformLayout();
         subscribersGroupBox.ResumeLayout(false);
         subscribersGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button m_CreateButton;
      private System.Windows.Forms.TextBox m_AddressTextBox;
      private System.Windows.Forms.DateTimePicker m_ExpirationTimePicker;
      private System.Windows.Forms.TextBox m_PushRetriesTextBox;
      private System.Windows.Forms.ComboBox m_OverflowComboBox;
      private System.Windows.Forms.TextBox m_BufferLengthTextBox;
      private System.Windows.Forms.RadioButton m_OneRadioButton;
      private System.Windows.Forms.RadioButton m_AllRadioButton;
      private System.Windows.Forms.TextBox m_MaxSubscribersTextBox;

   }
}