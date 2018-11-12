namespace ServiceModelEx
{
   partial class ContractViewControl
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
         System.Windows.Forms.GroupBox protectionGroupBox;
         System.Windows.Forms.GroupBox aspectsGroupBox;
         System.Windows.Forms.GroupBox sessionGroupBox;
         this.m_EncryptRadioButton = new System.Windows.Forms.RadioButton();
         this.m_SignedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NonProtectionRadioButton = new System.Windows.Forms.RadioButton();
         this.m_RequiredRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NotAllowedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_AllowedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NameLabel = new System.Windows.Forms.Label();
         this.m_NamespaceLabel = new System.Windows.Forms.Label();
         protectionGroupBox = new System.Windows.Forms.GroupBox();
         aspectsGroupBox = new System.Windows.Forms.GroupBox();
         sessionGroupBox = new System.Windows.Forms.GroupBox();
         protectionGroupBox.SuspendLayout();
         aspectsGroupBox.SuspendLayout();
         sessionGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // protectionGroupBox
         // 
         protectionGroupBox.Controls.Add(this.m_EncryptRadioButton);
         protectionGroupBox.Controls.Add(this.m_SignedRadioButton);
         protectionGroupBox.Controls.Add(this.m_NonProtectionRadioButton);
         protectionGroupBox.Location = new System.Drawing.Point(132,16);
         protectionGroupBox.Name = "protectionGroupBox";
         protectionGroupBox.Size = new System.Drawing.Size(118,94);
         protectionGroupBox.TabIndex = 2;
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
         // aspectsGroupBox
         // 
         aspectsGroupBox.Controls.Add(sessionGroupBox);
         aspectsGroupBox.Controls.Add(protectionGroupBox);
         aspectsGroupBox.Location = new System.Drawing.Point(15,77);
         aspectsGroupBox.Name = "aspectsGroupBox";
         aspectsGroupBox.Size = new System.Drawing.Size(261,121);
         aspectsGroupBox.TabIndex = 6;
         aspectsGroupBox.TabStop = false;
         aspectsGroupBox.Text = "Aspects";
         // 
         // sessionGroupBox
         // 
         sessionGroupBox.Controls.Add(this.m_RequiredRadioButton);
         sessionGroupBox.Controls.Add(this.m_NotAllowedRadioButton);
         sessionGroupBox.Controls.Add(this.m_AllowedRadioButton);
         sessionGroupBox.Location = new System.Drawing.Point(6,16);
         sessionGroupBox.Name = "sessionGroupBox";
         sessionGroupBox.Size = new System.Drawing.Size(120,94);
         sessionGroupBox.TabIndex = 1;
         sessionGroupBox.TabStop = false;
         sessionGroupBox.Text = "Session";
         // 
         // m_RequiredRadioButton
         // 
         this.m_RequiredRadioButton.AutoSize = true;
         this.m_RequiredRadioButton.Location = new System.Drawing.Point(6,63);
         this.m_RequiredRadioButton.Name = "m_RequiredRadioButton";
         this.m_RequiredRadioButton.Size = new System.Drawing.Size(68,17);
         this.m_RequiredRadioButton.TabIndex = 5;
         this.m_RequiredRadioButton.TabStop = true;
         this.m_RequiredRadioButton.Text = "Required";
         this.m_RequiredRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_NotAllowedRadioButton
         // 
         this.m_NotAllowedRadioButton.AutoSize = true;
         this.m_NotAllowedRadioButton.Location = new System.Drawing.Point(6,40);
         this.m_NotAllowedRadioButton.Name = "m_NotAllowedRadioButton";
         this.m_NotAllowedRadioButton.Size = new System.Drawing.Size(82,17);
         this.m_NotAllowedRadioButton.TabIndex = 4;
         this.m_NotAllowedRadioButton.TabStop = true;
         this.m_NotAllowedRadioButton.Text = "Not Allowed";
         this.m_NotAllowedRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_AllowedRadioButton
         // 
         this.m_AllowedRadioButton.AutoSize = true;
         this.m_AllowedRadioButton.Location = new System.Drawing.Point(6,17);
         this.m_AllowedRadioButton.Name = "m_AllowedRadioButton";
         this.m_AllowedRadioButton.Size = new System.Drawing.Size(62,17);
         this.m_AllowedRadioButton.TabIndex = 3;
         this.m_AllowedRadioButton.TabStop = true;
         this.m_AllowedRadioButton.Text = "Allowed";
         this.m_AllowedRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_NameLabel
         // 
         this.m_NameLabel.AutoSize = true;
         this.m_NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",10F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
         this.m_NameLabel.Location = new System.Drawing.Point(12,15);
         this.m_NameLabel.Name = "m_NameLabel";
         this.m_NameLabel.Size = new System.Drawing.Size(59,17);
         this.m_NameLabel.TabIndex = 3;
         this.m_NameLabel.Text = "Name: ";
         // 
         // m_NamespaceLabel
         // 
         this.m_NamespaceLabel.AutoSize = true;
         this.m_NamespaceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",10F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
         this.m_NamespaceLabel.Location = new System.Drawing.Point(12,43);
         this.m_NamespaceLabel.Name = "m_NamespaceLabel";
         this.m_NamespaceLabel.Size = new System.Drawing.Size(97,17);
         this.m_NamespaceLabel.TabIndex = 4;
         this.m_NamespaceLabel.Text = "Namespace:";
         // 
         // ContractViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(aspectsGroupBox);
         this.Controls.Add(this.m_NamespaceLabel);
         this.Controls.Add(this.m_NameLabel);
         this.Name = "ContractViewControl";
         protectionGroupBox.ResumeLayout(false);
         protectionGroupBox.PerformLayout();
         aspectsGroupBox.ResumeLayout(false);
         sessionGroupBox.ResumeLayout(false);
         sessionGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion

      private System.Windows.Forms.RadioButton m_EncryptRadioButton;
      private System.Windows.Forms.RadioButton m_SignedRadioButton;
      private System.Windows.Forms.RadioButton m_NonProtectionRadioButton;
      private System.Windows.Forms.Label m_NameLabel;
      private System.Windows.Forms.Label m_NamespaceLabel;
      private System.Windows.Forms.RadioButton m_RequiredRadioButton;
      private System.Windows.Forms.RadioButton m_NotAllowedRadioButton;
      private System.Windows.Forms.RadioButton m_AllowedRadioButton;


   }
}
