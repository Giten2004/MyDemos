namespace ServiceModelEx
{
   partial class BindingViewControl
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
         System.Windows.Forms.GroupBox credentailsGroup;
         System.Windows.Forms.GroupBox protectionGroupBox;
         System.Windows.Forms.GroupBox transferGroupBox;
         System.Windows.Forms.GroupBox deliveryGroubBox;
         System.Windows.Forms.GroupBox transactionsGroupBox;
         System.Windows.Forms.GroupBox groupBox1;
         this.securityGroupBox = new System.Windows.Forms.GroupBox();
         this.m_TokenRadioButton = new System.Windows.Forms.RadioButton();
         this.m_CertificateCredentialsRadioButton = new System.Windows.Forms.RadioButton();
         this.m_UsernameCredentialsRadioButton = new System.Windows.Forms.RadioButton();
         this.m_WindowsCredentialsRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NoCredentialsRadioButton = new System.Windows.Forms.RadioButton();
         this.m_EncryptRadioButton = new System.Windows.Forms.RadioButton();
         this.m_SignedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NonProtectionRadioButton = new System.Windows.Forms.RadioButton();
         this.m_BothRadioButton = new System.Windows.Forms.RadioButton();
         this.m_MixedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_MessageRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NoneRadioButton = new System.Windows.Forms.RadioButton();
         this.m_TransportRadioButton = new System.Windows.Forms.RadioButton();
         this.reliabilityGroupBox = new System.Windows.Forms.GroupBox();
         this.m_ReliabilityEnabledLabel = new System.Windows.Forms.Label();
         this.m_OrderedLabel = new System.Windows.Forms.Label();
         this.m_TransactionFlowLabel = new System.Windows.Forms.Label();
         this.m_TransactionProtocol = new System.Windows.Forms.Label();
         this.m_StreamingEnabledLabel = new System.Windows.Forms.Label();
         credentailsGroup = new System.Windows.Forms.GroupBox();
         protectionGroupBox = new System.Windows.Forms.GroupBox();
         transferGroupBox = new System.Windows.Forms.GroupBox();
         deliveryGroubBox = new System.Windows.Forms.GroupBox();
         transactionsGroupBox = new System.Windows.Forms.GroupBox();
         groupBox1 = new System.Windows.Forms.GroupBox();
         this.securityGroupBox.SuspendLayout();
         credentailsGroup.SuspendLayout();
         protectionGroupBox.SuspendLayout();
         transferGroupBox.SuspendLayout();
         deliveryGroubBox.SuspendLayout();
         this.reliabilityGroupBox.SuspendLayout();
         transactionsGroupBox.SuspendLayout();
         groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // securityGroupBox
         // 
         this.securityGroupBox.Controls.Add(credentailsGroup);
         this.securityGroupBox.Controls.Add(protectionGroupBox);
         this.securityGroupBox.Controls.Add(transferGroupBox);
         this.securityGroupBox.Location = new System.Drawing.Point(11,162);
         this.securityGroupBox.Name = "securityGroupBox";
         this.securityGroupBox.Size = new System.Drawing.Size(359,164);
         this.securityGroupBox.TabIndex = 0;
         this.securityGroupBox.TabStop = false;
         this.securityGroupBox.Text = "Security";
         // 
         // credentailsGroup
         // 
         credentailsGroup.Controls.Add(this.m_TokenRadioButton);
         credentailsGroup.Controls.Add(this.m_CertificateCredentialsRadioButton);
         credentailsGroup.Controls.Add(this.m_UsernameCredentialsRadioButton);
         credentailsGroup.Controls.Add(this.m_WindowsCredentialsRadioButton);
         credentailsGroup.Controls.Add(this.m_NoCredentialsRadioButton);
         credentailsGroup.Location = new System.Drawing.Point(109,19);
         credentailsGroup.Name = "credentailsGroup";
         credentailsGroup.Size = new System.Drawing.Size(120,136);
         credentailsGroup.TabIndex = 2;
         credentailsGroup.TabStop = false;
         credentailsGroup.Text = "Credentials Type";
         // 
         // m_TokenRadioButton
         // 
         this.m_TokenRadioButton.AutoSize = true;
         this.m_TokenRadioButton.Location = new System.Drawing.Point(6,108);
         this.m_TokenRadioButton.Name = "m_TokenRadioButton";
         this.m_TokenRadioButton.Size = new System.Drawing.Size(56,17);
         this.m_TokenRadioButton.TabIndex = 1;
         this.m_TokenRadioButton.TabStop = true;
         this.m_TokenRadioButton.Text = "Token";
         this.m_TokenRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_CertificateCredentialsRadioButton
         // 
         this.m_CertificateCredentialsRadioButton.AutoSize = true;
         this.m_CertificateCredentialsRadioButton.Location = new System.Drawing.Point(6,85);
         this.m_CertificateCredentialsRadioButton.Name = "m_CertificateCredentialsRadioButton";
         this.m_CertificateCredentialsRadioButton.Size = new System.Drawing.Size(72,17);
         this.m_CertificateCredentialsRadioButton.TabIndex = 1;
         this.m_CertificateCredentialsRadioButton.TabStop = true;
         this.m_CertificateCredentialsRadioButton.Text = "Certificate";
         this.m_CertificateCredentialsRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_UsernameCredentialsRadioButton
         // 
         this.m_UsernameCredentialsRadioButton.AutoSize = true;
         this.m_UsernameCredentialsRadioButton.Location = new System.Drawing.Point(6,62);
         this.m_UsernameCredentialsRadioButton.Name = "m_UsernameCredentialsRadioButton";
         this.m_UsernameCredentialsRadioButton.Size = new System.Drawing.Size(73,17);
         this.m_UsernameCredentialsRadioButton.TabIndex = 1;
         this.m_UsernameCredentialsRadioButton.TabStop = true;
         this.m_UsernameCredentialsRadioButton.Text = "Username";
         this.m_UsernameCredentialsRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_WindowsCredentialsRadioButton
         // 
         this.m_WindowsCredentialsRadioButton.AutoSize = true;
         this.m_WindowsCredentialsRadioButton.Location = new System.Drawing.Point(6,39);
         this.m_WindowsCredentialsRadioButton.Name = "m_WindowsCredentialsRadioButton";
         this.m_WindowsCredentialsRadioButton.Size = new System.Drawing.Size(69,17);
         this.m_WindowsCredentialsRadioButton.TabIndex = 1;
         this.m_WindowsCredentialsRadioButton.TabStop = true;
         this.m_WindowsCredentialsRadioButton.Text = "Windows";
         this.m_WindowsCredentialsRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_NoCredentialsRadioButton
         // 
         this.m_NoCredentialsRadioButton.AutoSize = true;
         this.m_NoCredentialsRadioButton.Location = new System.Drawing.Point(6,16);
         this.m_NoCredentialsRadioButton.Name = "m_NoCredentialsRadioButton";
         this.m_NoCredentialsRadioButton.Size = new System.Drawing.Size(51,17);
         this.m_NoCredentialsRadioButton.TabIndex = 0;
         this.m_NoCredentialsRadioButton.TabStop = true;
         this.m_NoCredentialsRadioButton.Text = "None";
         this.m_NoCredentialsRadioButton.UseVisualStyleBackColor = true;
         // 
         // protectionGroupBox
         // 
         protectionGroupBox.Controls.Add(this.m_EncryptRadioButton);
         protectionGroupBox.Controls.Add(this.m_SignedRadioButton);
         protectionGroupBox.Controls.Add(this.m_NonProtectionRadioButton);
         protectionGroupBox.Location = new System.Drawing.Point(235,19);
         protectionGroupBox.Name = "protectionGroupBox";
         protectionGroupBox.Size = new System.Drawing.Size(118,136);
         protectionGroupBox.TabIndex = 1;
         protectionGroupBox.TabStop = false;
         protectionGroupBox.Text = "Protection Level";
         // 
         // m_EncryptRadioButton
         // 
         this.m_EncryptRadioButton.AutoSize = true;
         this.m_EncryptRadioButton.Location = new System.Drawing.Point(6,63);
         this.m_EncryptRadioButton.Name = "m_EncryptRadioButton";
         this.m_EncryptRadioButton.Size = new System.Drawing.Size(106,17);
         this.m_EncryptRadioButton.TabIndex = 0;
         this.m_EncryptRadioButton.TabStop = true;
         this.m_EncryptRadioButton.Text = "Encrypt and Sign";
         this.m_EncryptRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_SignedRadioButton
         // 
         this.m_SignedRadioButton.AutoSize = true;
         this.m_SignedRadioButton.Location = new System.Drawing.Point(6,40);
         this.m_SignedRadioButton.Name = "m_SignedRadioButton";
         this.m_SignedRadioButton.Size = new System.Drawing.Size(58,17);
         this.m_SignedRadioButton.TabIndex = 0;
         this.m_SignedRadioButton.TabStop = true;
         this.m_SignedRadioButton.Text = "Signed";
         this.m_SignedRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_NonProtectionRadioButton
         // 
         this.m_NonProtectionRadioButton.AutoSize = true;
         this.m_NonProtectionRadioButton.Location = new System.Drawing.Point(6,17);
         this.m_NonProtectionRadioButton.Name = "m_NonProtectionRadioButton";
         this.m_NonProtectionRadioButton.Size = new System.Drawing.Size(51,17);
         this.m_NonProtectionRadioButton.TabIndex = 0;
         this.m_NonProtectionRadioButton.TabStop = true;
         this.m_NonProtectionRadioButton.Text = "None";
         this.m_NonProtectionRadioButton.UseVisualStyleBackColor = true;
         // 
         // transferGroupBox
         // 
         transferGroupBox.Controls.Add(this.m_BothRadioButton);
         transferGroupBox.Controls.Add(this.m_MixedRadioButton);
         transferGroupBox.Controls.Add(this.m_MessageRadioButton);
         transferGroupBox.Controls.Add(this.m_NoneRadioButton);
         transferGroupBox.Controls.Add(this.m_TransportRadioButton);
         transferGroupBox.Location = new System.Drawing.Point(7,20);
         transferGroupBox.Name = "transferGroupBox";
         transferGroupBox.Size = new System.Drawing.Size(96,135);
         transferGroupBox.TabIndex = 0;
         transferGroupBox.TabStop = false;
         transferGroupBox.Text = "Transfer Mode";
         // 
         // m_BothRadioButton
         // 
         this.m_BothRadioButton.AutoSize = true;
         this.m_BothRadioButton.Location = new System.Drawing.Point(6,109);
         this.m_BothRadioButton.Name = "m_BothRadioButton";
         this.m_BothRadioButton.Size = new System.Drawing.Size(47,17);
         this.m_BothRadioButton.TabIndex = 0;
         this.m_BothRadioButton.TabStop = true;
         this.m_BothRadioButton.Text = "Both";
         this.m_BothRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_MixedRadioButton
         // 
         this.m_MixedRadioButton.AutoSize = true;
         this.m_MixedRadioButton.Location = new System.Drawing.Point(6,86);
         this.m_MixedRadioButton.Name = "m_MixedRadioButton";
         this.m_MixedRadioButton.Size = new System.Drawing.Size(53,17);
         this.m_MixedRadioButton.TabIndex = 0;
         this.m_MixedRadioButton.TabStop = true;
         this.m_MixedRadioButton.Text = "Mixed";
         this.m_MixedRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_MessageRadioButton
         // 
         this.m_MessageRadioButton.AutoSize = true;
         this.m_MessageRadioButton.Location = new System.Drawing.Point(6,63);
         this.m_MessageRadioButton.Name = "m_MessageRadioButton";
         this.m_MessageRadioButton.Size = new System.Drawing.Size(68,17);
         this.m_MessageRadioButton.TabIndex = 0;
         this.m_MessageRadioButton.TabStop = true;
         this.m_MessageRadioButton.Text = "Message";
         this.m_MessageRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_NoneRadioButton
         // 
         this.m_NoneRadioButton.AutoSize = true;
         this.m_NoneRadioButton.Location = new System.Drawing.Point(6,17);
         this.m_NoneRadioButton.Name = "m_NoneRadioButton";
         this.m_NoneRadioButton.Size = new System.Drawing.Size(51,17);
         this.m_NoneRadioButton.TabIndex = 0;
         this.m_NoneRadioButton.TabStop = true;
         this.m_NoneRadioButton.Text = "None";
         this.m_NoneRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_TransportRadioButton
         // 
         this.m_TransportRadioButton.AutoSize = true;
         this.m_TransportRadioButton.Location = new System.Drawing.Point(6,40);
         this.m_TransportRadioButton.Name = "m_TransportRadioButton";
         this.m_TransportRadioButton.Size = new System.Drawing.Size(70,17);
         this.m_TransportRadioButton.TabIndex = 0;
         this.m_TransportRadioButton.TabStop = true;
         this.m_TransportRadioButton.Text = "Transport";
         this.m_TransportRadioButton.UseVisualStyleBackColor = true;
         // 
         // deliveryGroubBox
         // 
         deliveryGroubBox.Controls.Add(groupBox1);
         deliveryGroubBox.Controls.Add(transactionsGroupBox);
         deliveryGroubBox.Controls.Add(this.reliabilityGroupBox);
         deliveryGroubBox.Location = new System.Drawing.Point(11,3);
         deliveryGroubBox.Name = "deliveryGroubBox";
         deliveryGroubBox.Size = new System.Drawing.Size(359,153);
         deliveryGroubBox.TabIndex = 1;
         deliveryGroubBox.TabStop = false;
         deliveryGroubBox.Text = "Delivery Aspects";
         // 
         // reliabilityGroupBox
         // 
         this.reliabilityGroupBox.Controls.Add(this.m_OrderedLabel);
         this.reliabilityGroupBox.Controls.Add(this.m_ReliabilityEnabledLabel);
         this.reliabilityGroupBox.Location = new System.Drawing.Point(7,19);
         this.reliabilityGroupBox.Name = "reliabilityGroupBox";
         this.reliabilityGroupBox.Size = new System.Drawing.Size(96,128);
         this.reliabilityGroupBox.TabIndex = 2;
         this.reliabilityGroupBox.TabStop = false;
         this.reliabilityGroupBox.Text = "Reliability";
         // 
         // m_ReliabilityEnabledLabel
         // 
         this.m_ReliabilityEnabledLabel.AutoSize = true;
         this.m_ReliabilityEnabledLabel.Location = new System.Drawing.Point(7,23);
         this.m_ReliabilityEnabledLabel.Name = "m_ReliabilityEnabledLabel";
         this.m_ReliabilityEnabledLabel.Size = new System.Drawing.Size(52,13);
         this.m_ReliabilityEnabledLabel.TabIndex = 3;
         this.m_ReliabilityEnabledLabel.Text = "Enabled: ";
         // 
         // m_OrderedLabel
         // 
         this.m_OrderedLabel.AutoSize = true;
         this.m_OrderedLabel.Location = new System.Drawing.Point(6,47);
         this.m_OrderedLabel.Name = "m_OrderedLabel";
         this.m_OrderedLabel.Size = new System.Drawing.Size(51,13);
         this.m_OrderedLabel.TabIndex = 4;
         this.m_OrderedLabel.Text = "Ordered: ";
         // 
         // transactionsGroupBox
         // 
         transactionsGroupBox.Controls.Add(this.m_TransactionProtocol);
         transactionsGroupBox.Controls.Add(this.m_TransactionFlowLabel);
         transactionsGroupBox.Location = new System.Drawing.Point(109,19);
         transactionsGroupBox.Name = "transactionsGroupBox";
         transactionsGroupBox.Size = new System.Drawing.Size(120,128);
         transactionsGroupBox.TabIndex = 3;
         transactionsGroupBox.TabStop = false;
         transactionsGroupBox.Text = "Transactions";
         // 
         // m_TransactionFlowLabel
         // 
         this.m_TransactionFlowLabel.AutoSize = true;
         this.m_TransactionFlowLabel.Location = new System.Drawing.Point(6,23);
         this.m_TransactionFlowLabel.Name = "m_TransactionFlowLabel";
         this.m_TransactionFlowLabel.Size = new System.Drawing.Size(77,13);
         this.m_TransactionFlowLabel.TabIndex = 0;
         this.m_TransactionFlowLabel.Text = "Flow Enabled: ";
         // 
         // m_TransactionProtocol
         // 
         this.m_TransactionProtocol.AutoSize = true;
         this.m_TransactionProtocol.Location = new System.Drawing.Point(6,47);
         this.m_TransactionProtocol.Name = "m_TransactionProtocol";
         this.m_TransactionProtocol.Size = new System.Drawing.Size(52,13);
         this.m_TransactionProtocol.TabIndex = 1;
         this.m_TransactionProtocol.Text = "Protocol: ";
         // 
         // groupBox1
         // 
         groupBox1.Controls.Add(this.m_StreamingEnabledLabel);
         groupBox1.Location = new System.Drawing.Point(235,19);
         groupBox1.Name = "groupBox1";
         groupBox1.Size = new System.Drawing.Size(118,128);
         groupBox1.TabIndex = 3;
         groupBox1.TabStop = false;
         groupBox1.Text = "Streaming";
         // 
         // m_StreamingEnabledLabel
         // 
         this.m_StreamingEnabledLabel.AutoSize = true;
         this.m_StreamingEnabledLabel.Location = new System.Drawing.Point(6,23);
         this.m_StreamingEnabledLabel.Name = "m_StreamingEnabledLabel";
         this.m_StreamingEnabledLabel.Size = new System.Drawing.Size(52,13);
         this.m_StreamingEnabledLabel.TabIndex = 0;
         this.m_StreamingEnabledLabel.Text = "Enabled: ";
         // 
         // BindingViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(deliveryGroubBox);
         this.Controls.Add(this.securityGroupBox);
         this.Name = "BindingViewControl";
         this.securityGroupBox.ResumeLayout(false);
         credentailsGroup.ResumeLayout(false);
         credentailsGroup.PerformLayout();
         protectionGroupBox.ResumeLayout(false);
         protectionGroupBox.PerformLayout();
         transferGroupBox.ResumeLayout(false);
         transferGroupBox.PerformLayout();
         deliveryGroubBox.ResumeLayout(false);
         this.reliabilityGroupBox.ResumeLayout(false);
         this.reliabilityGroupBox.PerformLayout();
         transactionsGroupBox.ResumeLayout(false);
         transactionsGroupBox.PerformLayout();
         groupBox1.ResumeLayout(false);
         groupBox1.PerformLayout();
         this.ResumeLayout(false);

      }
      #endregion

      private System.Windows.Forms.GroupBox securityGroupBox;
      private System.Windows.Forms.RadioButton m_BothRadioButton;
      private System.Windows.Forms.RadioButton m_MixedRadioButton;
      private System.Windows.Forms.RadioButton m_MessageRadioButton;
      private System.Windows.Forms.RadioButton m_TransportRadioButton;
      private System.Windows.Forms.RadioButton m_NoneRadioButton;
      private System.Windows.Forms.RadioButton m_EncryptRadioButton;
      private System.Windows.Forms.RadioButton m_SignedRadioButton;
      private System.Windows.Forms.RadioButton m_NonProtectionRadioButton;
      private System.Windows.Forms.RadioButton m_WindowsCredentialsRadioButton;
      private System.Windows.Forms.RadioButton m_NoCredentialsRadioButton;
      private System.Windows.Forms.RadioButton m_TokenRadioButton;
      private System.Windows.Forms.RadioButton m_CertificateCredentialsRadioButton;
      private System.Windows.Forms.RadioButton m_UsernameCredentialsRadioButton;
      private System.Windows.Forms.Label m_TransactionProtocol;
      private System.Windows.Forms.Label m_TransactionFlowLabel;
      private System.Windows.Forms.GroupBox reliabilityGroupBox;
      private System.Windows.Forms.Label m_OrderedLabel;
      private System.Windows.Forms.Label m_ReliabilityEnabledLabel;
      private System.Windows.Forms.Label m_StreamingEnabledLabel;


   }
}
