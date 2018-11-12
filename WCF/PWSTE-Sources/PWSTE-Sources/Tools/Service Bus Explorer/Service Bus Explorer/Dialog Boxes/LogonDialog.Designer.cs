namespace ServiceModelEx
{
   partial class LogonDialog
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
         System.Windows.Forms.GroupBox credsTypeGroupBox;
         System.Windows.Forms.Label storeNameLabel;
         System.Windows.Forms.GroupBox secretGroubBox;
         System.Windows.Forms.Label secretLabel;
         System.Windows.Forms.Label issuerLabel;
         System.Windows.Forms.GroupBox certificateGroupBox;
         System.Windows.Forms.Label storeLable;
         System.Windows.Forms.Label findValueLabel;
         System.Windows.Forms.Label certValueLabel;
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogonDialog));
         this.m_CertRadioButton = new System.Windows.Forms.RadioButton();
         this.m_SecretRadioButton = new System.Windows.Forms.RadioButton();
         this.m_CardSpaceRadioButton = new System.Windows.Forms.RadioButton();
         this.m_SecretTextBox = new System.Windows.Forms.TextBox();
         this.m_IssuerTextBox = new System.Windows.Forms.TextBox();
         this.m_StoreNameComboBox = new System.Windows.Forms.ComboBox();
         this.m_StoreLoctionComboBox = new System.Windows.Forms.ComboBox();
         this.m_FindValueComboBox = new System.Windows.Forms.ComboBox();
         this.m_CertNValueTextBox = new System.Windows.Forms.TextBox();
         this.m_LogonButton = new System.Windows.Forms.Button();
         credsTypeGroupBox = new System.Windows.Forms.GroupBox();
         storeNameLabel = new System.Windows.Forms.Label();
         secretGroubBox = new System.Windows.Forms.GroupBox();
         secretLabel = new System.Windows.Forms.Label();
         issuerLabel = new System.Windows.Forms.Label();
         certificateGroupBox = new System.Windows.Forms.GroupBox();
         storeLable = new System.Windows.Forms.Label();
         findValueLabel = new System.Windows.Forms.Label();
         certValueLabel = new System.Windows.Forms.Label();
         credsTypeGroupBox.SuspendLayout();
         secretGroubBox.SuspendLayout();
         certificateGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // credsTypeGroupBox
         // 
         credsTypeGroupBox.Controls.Add(this.m_CertRadioButton);
         credsTypeGroupBox.Controls.Add(this.m_SecretRadioButton);
         credsTypeGroupBox.Controls.Add(this.m_CardSpaceRadioButton);
         credsTypeGroupBox.Enabled = false;
         credsTypeGroupBox.Location = new System.Drawing.Point(17,17);
         credsTypeGroupBox.Name = "credsTypeGroupBox";
         credsTypeGroupBox.Size = new System.Drawing.Size(180,89);
         credsTypeGroupBox.TabIndex = 8;
         credsTypeGroupBox.TabStop = false;
         credsTypeGroupBox.Text = "Credentials Type";
         // 
         // m_CertRadioButton
         // 
         this.m_CertRadioButton.AutoSize = true;
         this.m_CertRadioButton.Enabled = false;
         this.m_CertRadioButton.Location = new System.Drawing.Point(6,65);
         this.m_CertRadioButton.Name = "m_CertRadioButton";
         this.m_CertRadioButton.Size = new System.Drawing.Size(72,17);
         this.m_CertRadioButton.TabIndex = 7;
         this.m_CertRadioButton.Text = "Certificate";
         this.m_CertRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_SecretRadioButton
         // 
         this.m_SecretRadioButton.AutoSize = true;
         this.m_SecretRadioButton.Checked = true;
         this.m_SecretRadioButton.Enabled = false;
         this.m_SecretRadioButton.Location = new System.Drawing.Point(6,42);
         this.m_SecretRadioButton.Name = "m_SecretRadioButton";
         this.m_SecretRadioButton.Size = new System.Drawing.Size(93,17);
         this.m_SecretRadioButton.TabIndex = 6;
         this.m_SecretRadioButton.TabStop = true;
         this.m_SecretRadioButton.Text = "Shared Secret";
         this.m_SecretRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_CardSpaceRadioButton
         // 
         this.m_CardSpaceRadioButton.AutoSize = true;
         this.m_CardSpaceRadioButton.Enabled = false;
         this.m_CardSpaceRadioButton.Location = new System.Drawing.Point(6,19);
         this.m_CardSpaceRadioButton.Name = "m_CardSpaceRadioButton";
         this.m_CardSpaceRadioButton.Size = new System.Drawing.Size(78,17);
         this.m_CardSpaceRadioButton.TabIndex = 5;
         this.m_CardSpaceRadioButton.Text = "CardSpace";
         this.m_CardSpaceRadioButton.UseVisualStyleBackColor = true;
         // 
         // storeNameLabel
         // 
         storeNameLabel.AutoSize = true;
         storeNameLabel.Location = new System.Drawing.Point(6,156);
         storeNameLabel.Name = "storeNameLabel";
         storeNameLabel.Size = new System.Drawing.Size(66,13);
         storeNameLabel.TabIndex = 6;
         storeNameLabel.Text = "Store Name:";
         // 
         // secretGroubBox
         // 
         secretGroubBox.Controls.Add(this.m_SecretTextBox);
         secretGroubBox.Controls.Add(secretLabel);
         secretGroubBox.Controls.Add(this.m_IssuerTextBox);
         secretGroubBox.Controls.Add(issuerLabel);
         secretGroubBox.Location = new System.Drawing.Point(17,112);
         secretGroubBox.Name = "secretGroubBox";
         secretGroubBox.Size = new System.Drawing.Size(180,116);
         secretGroubBox.TabIndex = 6;
         secretGroubBox.TabStop = false;
         secretGroubBox.Text = "Shared Secret";
         // 
         // m_SecretTextBox
         // 
         this.m_SecretTextBox.Location = new System.Drawing.Point(6,78);
         this.m_SecretTextBox.Name = "m_SecretTextBox";
         this.m_SecretTextBox.PasswordChar = '*';
         this.m_SecretTextBox.Size = new System.Drawing.Size(157,20);
         this.m_SecretTextBox.TabIndex = 3;
         this.m_SecretTextBox.Text = "******** Enter your secret here *******";
         this.m_SecretTextBox.UseSystemPasswordChar = true;
         this.m_SecretTextBox.TextChanged += new System.EventHandler(this.OnTextChanged);
         // 
         // secretLabel
         // 
         secretLabel.AutoSize = true;
         secretLabel.Location = new System.Drawing.Point(3,62);
         secretLabel.Name = "secretLabel";
         secretLabel.Size = new System.Drawing.Size(41,13);
         secretLabel.TabIndex = 2;
         secretLabel.Text = "Secret:";
         // 
         // m_IssuerTextBox
         // 
         this.m_IssuerTextBox.Enabled = false;
         this.m_IssuerTextBox.Location = new System.Drawing.Point(6,35);
         this.m_IssuerTextBox.Name = "m_IssuerTextBox";
         this.m_IssuerTextBox.Size = new System.Drawing.Size(157,20);
         this.m_IssuerTextBox.TabIndex = 1;
         // 
         // issuerLabel
         // 
         issuerLabel.AutoSize = true;
         issuerLabel.Location = new System.Drawing.Point(3,19);
         issuerLabel.Name = "issuerLabel";
         issuerLabel.Size = new System.Drawing.Size(38,13);
         issuerLabel.TabIndex = 0;
         issuerLabel.Text = "Issuer:";
         // 
         // certificateGroupBox
         // 
         certificateGroupBox.Controls.Add(this.m_StoreNameComboBox);
         certificateGroupBox.Controls.Add(storeNameLabel);
         certificateGroupBox.Controls.Add(this.m_StoreLoctionComboBox);
         certificateGroupBox.Controls.Add(storeLable);
         certificateGroupBox.Controls.Add(this.m_FindValueComboBox);
         certificateGroupBox.Controls.Add(findValueLabel);
         certificateGroupBox.Controls.Add(this.m_CertNValueTextBox);
         certificateGroupBox.Controls.Add(certValueLabel);
         certificateGroupBox.Enabled = false;
         certificateGroupBox.Location = new System.Drawing.Point(216,17);
         certificateGroupBox.Name = "certificateGroupBox";
         certificateGroupBox.Size = new System.Drawing.Size(143,211);
         certificateGroupBox.TabIndex = 5;
         certificateGroupBox.TabStop = false;
         certificateGroupBox.Text = "Certificate:";
         // 
         // m_StoreNameComboBox
         // 
         this.m_StoreNameComboBox.Enabled = false;
         this.m_StoreNameComboBox.FormattingEnabled = true;
         this.m_StoreNameComboBox.Items.AddRange(new object[] {
            "My"});
         this.m_StoreNameComboBox.Location = new System.Drawing.Point(6,172);
         this.m_StoreNameComboBox.Name = "m_StoreNameComboBox";
         this.m_StoreNameComboBox.Size = new System.Drawing.Size(121,21);
         this.m_StoreNameComboBox.TabIndex = 7;
         // 
         // m_StoreLoctionComboBox
         // 
         this.m_StoreLoctionComboBox.Enabled = false;
         this.m_StoreLoctionComboBox.FormattingEnabled = true;
         this.m_StoreLoctionComboBox.Items.AddRange(new object[] {
            "Local Machine"});
         this.m_StoreLoctionComboBox.Location = new System.Drawing.Point(6,129);
         this.m_StoreLoctionComboBox.Name = "m_StoreLoctionComboBox";
         this.m_StoreLoctionComboBox.Size = new System.Drawing.Size(121,21);
         this.m_StoreLoctionComboBox.TabIndex = 5;
         // 
         // storeLable
         // 
         storeLable.AutoSize = true;
         storeLable.Location = new System.Drawing.Point(6,113);
         storeLable.Name = "storeLable";
         storeLable.Size = new System.Drawing.Size(79,13);
         storeLable.TabIndex = 4;
         storeLable.Text = "Store Location:";
         // 
         // m_FindValueComboBox
         // 
         this.m_FindValueComboBox.Enabled = false;
         this.m_FindValueComboBox.FormattingEnabled = true;
         this.m_FindValueComboBox.Items.AddRange(new object[] {
            "By Subject Name"});
         this.m_FindValueComboBox.Location = new System.Drawing.Point(6,89);
         this.m_FindValueComboBox.Name = "m_FindValueComboBox";
         this.m_FindValueComboBox.Size = new System.Drawing.Size(121,21);
         this.m_FindValueComboBox.TabIndex = 3;
         // 
         // findValueLabel
         // 
         findValueLabel.AutoSize = true;
         findValueLabel.Location = new System.Drawing.Point(3,73);
         findValueLabel.Name = "findValueLabel";
         findValueLabel.Size = new System.Drawing.Size(75,13);
         findValueLabel.TabIndex = 2;
         findValueLabel.Text = "Find Value By:";
         // 
         // m_CertNValueTextBox
         // 
         this.m_CertNValueTextBox.Enabled = false;
         this.m_CertNValueTextBox.Location = new System.Drawing.Point(6,35);
         this.m_CertNValueTextBox.Name = "m_CertNValueTextBox";
         this.m_CertNValueTextBox.Size = new System.Drawing.Size(124,20);
         this.m_CertNValueTextBox.TabIndex = 1;
         // 
         // certValueLabel
         // 
         certValueLabel.AutoSize = true;
         certValueLabel.Location = new System.Drawing.Point(3,16);
         certValueLabel.Name = "certValueLabel";
         certValueLabel.Size = new System.Drawing.Size(87,13);
         certValueLabel.TabIndex = 0;
         certValueLabel.Text = "Certificate Value:";
         // 
         // m_LogonButton
         // 
         this.m_LogonButton.Enabled = false;
         this.m_LogonButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
         this.m_LogonButton.Location = new System.Drawing.Point(281,247);
         this.m_LogonButton.Name = "m_LogonButton";
         this.m_LogonButton.Size = new System.Drawing.Size(78,23);
         this.m_LogonButton.TabIndex = 7;
         this.m_LogonButton.Text = "Login";
         this.m_LogonButton.UseVisualStyleBackColor = true;
         this.m_LogonButton.Click += new System.EventHandler(this.OnLogon);
         // 
         // LogonDialog
         // 
         this.AcceptButton = this.m_LogonButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(377,287);
         this.Controls.Add(credsTypeGroupBox);
         this.Controls.Add(this.m_LogonButton);
         this.Controls.Add(secretGroubBox);
         this.Controls.Add(certificateGroupBox);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "LogonDialog";
         this.Text = "Logon to the AppFabric Service Bus";
         credsTypeGroupBox.ResumeLayout(false);
         credsTypeGroupBox.PerformLayout();
         secretGroubBox.ResumeLayout(false);
         secretGroubBox.PerformLayout();
         certificateGroupBox.ResumeLayout(false);
         certificateGroupBox.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.RadioButton m_CertRadioButton;
      private System.Windows.Forms.RadioButton m_SecretRadioButton;
      private System.Windows.Forms.RadioButton m_CardSpaceRadioButton;
      private System.Windows.Forms.ComboBox m_StoreNameComboBox;
      private System.Windows.Forms.TextBox m_SecretTextBox;
      private System.Windows.Forms.TextBox m_IssuerTextBox;
      private System.Windows.Forms.ComboBox m_StoreLoctionComboBox;
      private System.Windows.Forms.ComboBox m_FindValueComboBox;
      private System.Windows.Forms.TextBox m_CertNValueTextBox;
      private System.Windows.Forms.Button m_LogonButton;

   }
}