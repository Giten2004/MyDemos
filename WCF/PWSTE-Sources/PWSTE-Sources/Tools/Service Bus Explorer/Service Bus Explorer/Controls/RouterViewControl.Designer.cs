// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

namespace ServiceModelEx.ServiceBus
{
   partial class RouterViewControl
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
         this.components = new System.ComponentModel.Container();
         System.Windows.Forms.Button deleteButton;
         System.Windows.Forms.Label bufferLengthLabel;
         System.Windows.Forms.Label maxSubscribersLabel;
         System.Windows.Forms.GroupBox subscribersGroupBox;
         System.Windows.Forms.Label overflowLabel;
         System.Windows.Forms.Label pushLabel;
         System.Windows.Forms.GroupBox routerGroupBox;
         System.Windows.Forms.Label renewLabel;
         System.Windows.Forms.Timer dirtyTimer;
         this.m_OneRadioButton = new System.Windows.Forms.RadioButton();
         this.m_AllRadioButton = new System.Windows.Forms.RadioButton();
         this.m_MaxSubscribersTextBox = new System.Windows.Forms.TextBox();
         this.m_PurgeButton = new System.Windows.Forms.Button();
         this.m_ExpirationTimePicker = new System.Windows.Forms.DateTimePicker();
         this.m_PushRetriesTextBox = new System.Windows.Forms.TextBox();
         this.m_OverflowComboBox = new System.Windows.Forms.ComboBox();
         this.m_BufferLengthTextBox = new System.Windows.Forms.TextBox();
         this.m_UpdateButton = new System.Windows.Forms.Button();
         this.m_ResetButton = new System.Windows.Forms.Button();
         deleteButton = new System.Windows.Forms.Button();
         bufferLengthLabel = new System.Windows.Forms.Label();
         maxSubscribersLabel = new System.Windows.Forms.Label();
         subscribersGroupBox = new System.Windows.Forms.GroupBox();
         overflowLabel = new System.Windows.Forms.Label();
         pushLabel = new System.Windows.Forms.Label();
         routerGroupBox = new System.Windows.Forms.GroupBox();
         renewLabel = new System.Windows.Forms.Label();
         dirtyTimer = new System.Windows.Forms.Timer(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).BeginInit();
         subscribersGroupBox.SuspendLayout();
         routerGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // m_AddressLabel
         // 
         this.m_AddressLabel.Visible = true;
         // 
         // m_ItemNameLabel
         // 
         this.m_ItemNameLabel.Size = new System.Drawing.Size(105,24);
         this.m_ItemNameLabel.Text = "My Router";
         this.m_ItemNameLabel.Visible = true;
         // 
         // m_ControlPictureBox
         // 
         this.m_ControlPictureBox.Location = new System.Drawing.Point(316,270);
         this.m_ControlPictureBox.Visible = true;
         // 
         // m_ControlAddressCaptionLabel
         // 
         this.m_ControlAddressCaptionLabel.Size = new System.Drawing.Size(98,13);
         this.m_ControlAddressCaptionLabel.Text = "Router Address:";
         this.m_ControlAddressCaptionLabel.Visible = true;
         // 
         // m_CopyButton
         // 
         this.m_CopyButton.Visible = true;
         // 
         // deleteButton
         // 
         deleteButton.Location = new System.Drawing.Point(172,301);
         deleteButton.Name = "deleteButton";
         deleteButton.Size = new System.Drawing.Size(75,23);
         deleteButton.TabIndex = 10;
         deleteButton.Text = "Delete";
         deleteButton.UseVisualStyleBackColor = true;
         deleteButton.Click += new System.EventHandler(this.OnDelete);
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
         // maxSubscribersLabel
         // 
         maxSubscribersLabel.AutoSize = true;
         maxSubscribersLabel.Location = new System.Drawing.Point(4,65);
         maxSubscribersLabel.Name = "maxSubscribersLabel";
         maxSubscribersLabel.Size = new System.Drawing.Size(88,13);
         maxSubscribersLabel.TabIndex = 2;
         maxSubscribersLabel.Text = "Max Subscribers:";
         // 
         // subscribersGroupBox
         // 
         subscribersGroupBox.Controls.Add(this.m_OneRadioButton);
         subscribersGroupBox.Controls.Add(this.m_AllRadioButton);
         subscribersGroupBox.Controls.Add(this.m_MaxSubscribersTextBox);
         subscribersGroupBox.Controls.Add(maxSubscribersLabel);
         subscribersGroupBox.Controls.Add(this.m_PurgeButton);
         subscribersGroupBox.Location = new System.Drawing.Point(11,109);
         subscribersGroupBox.Name = "subscribersGroupBox";
         subscribersGroupBox.Size = new System.Drawing.Size(115,143);
         subscribersGroupBox.TabIndex = 12;
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
         this.m_MaxSubscribersTextBox.Location = new System.Drawing.Point(6,82);
         this.m_MaxSubscribersTextBox.Name = "m_MaxSubscribersTextBox";
         this.m_MaxSubscribersTextBox.Size = new System.Drawing.Size(96,20);
         this.m_MaxSubscribersTextBox.TabIndex = 3;
         // 
         // m_PurgeButton
         // 
         this.m_PurgeButton.Enabled = false;
         this.m_PurgeButton.Location = new System.Drawing.Point(7,110);
         this.m_PurgeButton.Name = "m_PurgeButton";
         this.m_PurgeButton.Size = new System.Drawing.Size(95,23);
         this.m_PurgeButton.TabIndex = 11;
         this.m_PurgeButton.Text = "Purge All";
         this.m_PurgeButton.UseVisualStyleBackColor = true;
         this.m_PurgeButton.Click += new System.EventHandler(this.OnPurge);
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
         // pushLabel
         // 
         pushLabel.AutoSize = true;
         pushLabel.Location = new System.Drawing.Point(8,25);
         pushLabel.Name = "pushLabel";
         pushLabel.Size = new System.Drawing.Size(84,13);
         pushLabel.TabIndex = 15;
         pushLabel.Text = "Delivery Retries:";
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
         routerGroupBox.Location = new System.Drawing.Point(138,109);
         routerGroupBox.Name = "routerGroupBox";
         routerGroupBox.Size = new System.Drawing.Size(235,143);
         routerGroupBox.TabIndex = 12;
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
         // m_PushRetriesTextBox
         // 
         this.m_PushRetriesTextBox.Location = new System.Drawing.Point(11,41);
         this.m_PushRetriesTextBox.Name = "m_PushRetriesTextBox";
         this.m_PushRetriesTextBox.Size = new System.Drawing.Size(103,20);
         this.m_PushRetriesTextBox.TabIndex = 16;
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
         // m_BufferLengthTextBox
         // 
         this.m_BufferLengthTextBox.Location = new System.Drawing.Point(11,89);
         this.m_BufferLengthTextBox.Name = "m_BufferLengthTextBox";
         this.m_BufferLengthTextBox.Size = new System.Drawing.Size(103,20);
         this.m_BufferLengthTextBox.TabIndex = 1;
         // 
         // dirtyTimer
         // 
         dirtyTimer.Enabled = true;
         dirtyTimer.Interval = 250;
         dirtyTimer.Tick += new System.EventHandler(this.OnTimerTick);
         // 
         // m_UpdateButton
         // 
         this.m_UpdateButton.Enabled = false;
         this.m_UpdateButton.Location = new System.Drawing.Point(10,301);
         this.m_UpdateButton.Name = "m_UpdateButton";
         this.m_UpdateButton.Size = new System.Drawing.Size(75,23);
         this.m_UpdateButton.TabIndex = 11;
         this.m_UpdateButton.Text = "Update";
         this.m_UpdateButton.UseVisualStyleBackColor = true;
         this.m_UpdateButton.Click += new System.EventHandler(this.OnUpdate);
         // 
         // m_ResetButton
         // 
         this.m_ResetButton.Location = new System.Drawing.Point(91,301);
         this.m_ResetButton.Name = "m_ResetButton";
         this.m_ResetButton.Size = new System.Drawing.Size(75,23);
         this.m_ResetButton.TabIndex = 11;
         this.m_ResetButton.Text = "Reset";
         this.m_ResetButton.UseVisualStyleBackColor = true;
         this.m_ResetButton.Click += new System.EventHandler(this.OnReset);
         // 
         // RouterViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(routerGroupBox);
         this.Controls.Add(subscribersGroupBox);
         this.Controls.Add(this.m_ResetButton);
         this.Controls.Add(this.m_UpdateButton);
         this.Controls.Add(deleteButton);
         this.Name = "RouterViewControl";
         this.Controls.SetChildIndex(this.m_CopyButton,0);
         this.Controls.SetChildIndex(this.m_AddressLabel,0);
         this.Controls.SetChildIndex(deleteButton,0);
         this.Controls.SetChildIndex(this.m_ControlAddressCaptionLabel,0);
         this.Controls.SetChildIndex(this.m_UpdateButton,0);
         this.Controls.SetChildIndex(this.m_ResetButton,0);
         this.Controls.SetChildIndex(subscribersGroupBox,0);
         this.Controls.SetChildIndex(routerGroupBox,0);
         this.Controls.SetChildIndex(this.m_ItemNameLabel,0);
         this.Controls.SetChildIndex(this.m_ControlPictureBox,0);
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).EndInit();
         subscribersGroupBox.ResumeLayout(false);
         subscribersGroupBox.PerformLayout();
         routerGroupBox.ResumeLayout(false);
         routerGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion
      private System.Windows.Forms.Button m_UpdateButton;
      private System.Windows.Forms.Button m_ResetButton;
      private System.Windows.Forms.Button m_PurgeButton;
      private System.Windows.Forms.TextBox m_BufferLengthTextBox;
      private System.Windows.Forms.TextBox m_MaxSubscribersTextBox;
      private System.Windows.Forms.RadioButton m_OneRadioButton;
      private System.Windows.Forms.RadioButton m_AllRadioButton;
      private System.Windows.Forms.ComboBox m_OverflowComboBox;
      private System.Windows.Forms.TextBox m_PushRetriesTextBox;
      private System.Windows.Forms.DateTimePicker m_ExpirationTimePicker;
   }
}
