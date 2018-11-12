// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

namespace ServiceModelEx.ServiceBus
{
   partial class BufferViewControl
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BufferViewControl));
         System.Windows.Forms.Timer dirtyTimer;
         System.Windows.Forms.Label renewLabel;
         System.Windows.Forms.Label overflowLabel;
         System.Windows.Forms.Button deleteButton;
         System.Windows.Forms.Label countLabel;
         this.BufferGroupBox = new System.Windows.Forms.GroupBox();
         this.m_ExpirationTime = new System.Windows.Forms.TextBox();
         this.m_ResetButton = new System.Windows.Forms.Button();
         this.m_UpdateButton = new System.Windows.Forms.Button();
         this.m_OverflowComboBox = new System.Windows.Forms.ComboBox();
         this.m_CountTextBox = new System.Windows.Forms.TextBox();
         this.m_PurgeButton = new System.Windows.Forms.Button();
         dirtyTimer = new System.Windows.Forms.Timer(this.components);
         renewLabel = new System.Windows.Forms.Label();
         overflowLabel = new System.Windows.Forms.Label();
         deleteButton = new System.Windows.Forms.Button();
         countLabel = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).BeginInit();
         this.BufferGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // m_AddressLabel
         // 
         this.m_AddressLabel.Size = new System.Drawing.Size(207,13);
         this.m_AddressLabel.Text = "sb://<namespace>.servicebus.appfabriclabs.com/...";
         this.m_AddressLabel.Visible = true;
         // 
         // m_ItemNameLabel
         // 
         this.m_ItemNameLabel.Size = new System.Drawing.Size(97,24);
         this.m_ItemNameLabel.Text = "My Buffer";
         this.m_ItemNameLabel.Visible = true;
         // 
         // m_ControlPictureBox
         // 
         this.m_ControlPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("m_ControlPictureBox.Image")));
         this.m_ControlPictureBox.Size = new System.Drawing.Size(59,60);
         this.m_ControlPictureBox.Visible = true;
         // 
         // m_ControlAddressCaptionLabel
         // 
         this.m_ControlAddressCaptionLabel.Size = new System.Drawing.Size(97,13);
         this.m_ControlAddressCaptionLabel.Text = "Buffer Address:";
         this.m_ControlAddressCaptionLabel.Visible = true;
         // 
         // m_CopyButton
         // 
         this.m_CopyButton.Visible = true;
         // 
         // dirtyTimer
         // 
         dirtyTimer.Enabled = true;
         dirtyTimer.Interval = 250;
         dirtyTimer.Tick += new System.EventHandler(this.OnTimerTick);
         // 
         // BufferGroupBox
         // 
         this.BufferGroupBox.Controls.Add(this.m_ExpirationTime);
         this.BufferGroupBox.Controls.Add(this.m_ResetButton);
         this.BufferGroupBox.Controls.Add(renewLabel);
         this.BufferGroupBox.Controls.Add(this.m_UpdateButton);
         this.BufferGroupBox.Controls.Add(overflowLabel);
         this.BufferGroupBox.Controls.Add(deleteButton);
         this.BufferGroupBox.Controls.Add(this.m_OverflowComboBox);
         this.BufferGroupBox.Controls.Add(countLabel);
         this.BufferGroupBox.Controls.Add(this.m_CountTextBox);
         this.BufferGroupBox.Controls.Add(this.m_PurgeButton);
         this.BufferGroupBox.Location = new System.Drawing.Point(10,102);
         this.BufferGroupBox.Name = "BufferGroupBox";
         this.BufferGroupBox.Size = new System.Drawing.Size(363,131);
         this.BufferGroupBox.TabIndex = 27;
         this.BufferGroupBox.TabStop = false;
         this.BufferGroupBox.Text = "Buffer";
         // 
         // m_ExpirationTime
         // 
         this.m_ExpirationTime.Location = new System.Drawing.Point(238,35);
         this.m_ExpirationTime.Name = "m_ExpirationTime";
         this.m_ExpirationTime.Size = new System.Drawing.Size(113,20);
         this.m_ExpirationTime.TabIndex = 25;
         this.m_ExpirationTime.Text = "5";
         // 
         // m_ResetButton
         // 
         this.m_ResetButton.Location = new System.Drawing.Point(99,89);
         this.m_ResetButton.Name = "m_ResetButton";
         this.m_ResetButton.Size = new System.Drawing.Size(75,23);
         this.m_ResetButton.TabIndex = 25;
         this.m_ResetButton.Text = "Reset";
         this.m_ResetButton.UseVisualStyleBackColor = true;
         this.m_ResetButton.Click += new System.EventHandler(this.OnReset);
         // 
         // renewLabel
         // 
         renewLabel.AutoSize = true;
         renewLabel.Location = new System.Drawing.Point(235,18);
         renewLabel.Name = "renewLabel";
         renewLabel.Size = new System.Drawing.Size(101,13);
         renewLabel.TabIndex = 24;
         renewLabel.Text = "Expiration (minutes):";
         // 
         // m_UpdateButton
         // 
         this.m_UpdateButton.Enabled = false;
         this.m_UpdateButton.Location = new System.Drawing.Point(11,89);
         this.m_UpdateButton.Name = "m_UpdateButton";
         this.m_UpdateButton.Size = new System.Drawing.Size(75,23);
         this.m_UpdateButton.TabIndex = 24;
         this.m_UpdateButton.Text = "Update";
         this.m_UpdateButton.UseVisualStyleBackColor = true;
         this.m_UpdateButton.Click += new System.EventHandler(this.OnUpdate);
         // 
         // overflowLabel
         // 
         overflowLabel.AutoSize = true;
         overflowLabel.Location = new System.Drawing.Point(118,19);
         overflowLabel.Name = "overflowLabel";
         overflowLabel.Size = new System.Drawing.Size(52,13);
         overflowLabel.TabIndex = 23;
         overflowLabel.Text = "Overflow:";
         // 
         // deleteButton
         // 
         deleteButton.Location = new System.Drawing.Point(187,89);
         deleteButton.Name = "deleteButton";
         deleteButton.Size = new System.Drawing.Size(75,23);
         deleteButton.TabIndex = 23;
         deleteButton.Text = "Delete";
         deleteButton.UseVisualStyleBackColor = true;
         deleteButton.Click += new System.EventHandler(this.OnDelete);
         // 
         // m_OverflowComboBox
         // 
         this.m_OverflowComboBox.FormattingEnabled = true;
         this.m_OverflowComboBox.Items.AddRange(new object[] {
            "Reject"});
         this.m_OverflowComboBox.Location = new System.Drawing.Point(121,35);
         this.m_OverflowComboBox.Name = "m_OverflowComboBox";
         this.m_OverflowComboBox.Size = new System.Drawing.Size(106,21);
         this.m_OverflowComboBox.TabIndex = 22;
         // 
         // countLabel
         // 
         countLabel.AutoSize = true;
         countLabel.Location = new System.Drawing.Point(6,19);
         countLabel.Name = "countLabel";
         countLabel.Size = new System.Drawing.Size(38,13);
         countLabel.TabIndex = 20;
         countLabel.Text = "Count:";
         // 
         // m_CountTextBox
         // 
         this.m_CountTextBox.Location = new System.Drawing.Point(9,35);
         this.m_CountTextBox.Name = "m_CountTextBox";
         this.m_CountTextBox.Size = new System.Drawing.Size(103,20);
         this.m_CountTextBox.TabIndex = 21;
         this.m_CountTextBox.Text = "10";
         // 
         // m_PurgeButton
         // 
         this.m_PurgeButton.Location = new System.Drawing.Point(275,89);
         this.m_PurgeButton.Name = "m_PurgeButton";
         this.m_PurgeButton.Size = new System.Drawing.Size(76,23);
         this.m_PurgeButton.TabIndex = 11;
         this.m_PurgeButton.Text = "Purge All";
         this.m_PurgeButton.UseVisualStyleBackColor = true;
         this.m_PurgeButton.Click += new System.EventHandler(this.OnPurge);
         // 
         // BufferViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(this.BufferGroupBox);
         this.Name = "BufferViewControl";
         this.Controls.SetChildIndex(this.m_ControlAddressCaptionLabel,0);
         this.Controls.SetChildIndex(this.m_CopyButton,0);
         this.Controls.SetChildIndex(this.m_ItemNameLabel,0);
         this.Controls.SetChildIndex(this.m_AddressLabel,0);
         this.Controls.SetChildIndex(this.m_ControlPictureBox,0);
         this.Controls.SetChildIndex(this.BufferGroupBox,0);
         ((System.ComponentModel.ISupportInitialize)(this.m_ControlPictureBox)).EndInit();
         this.BufferGroupBox.ResumeLayout(false);
         this.BufferGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion

      private System.Windows.Forms.GroupBox BufferGroupBox;
      private System.Windows.Forms.TextBox m_ExpirationTime;
      private System.Windows.Forms.Button m_ResetButton;
      private System.Windows.Forms.Button m_UpdateButton;
      private System.Windows.Forms.ComboBox m_OverflowComboBox;
      private System.Windows.Forms.TextBox m_CountTextBox;
      private System.Windows.Forms.Button m_PurgeButton;
   }
}
