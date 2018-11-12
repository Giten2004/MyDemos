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
         System.Windows.Forms.GroupBox secretGroubBox;
         System.Windows.Forms.Label secretLabel;
         System.Windows.Forms.Label issuerLabel;
         System.Windows.Forms.Label namespaceLabel;
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogonDialog));
         this.m_SecretTextBox = new System.Windows.Forms.TextBox();
         this.m_IssuerTextBox = new System.Windows.Forms.TextBox();
         this.m_LogonButton = new System.Windows.Forms.Button();
         this.m_NamespaceTextbox = new System.Windows.Forms.TextBox();
         secretGroubBox = new System.Windows.Forms.GroupBox();
         secretLabel = new System.Windows.Forms.Label();
         issuerLabel = new System.Windows.Forms.Label();
         namespaceLabel = new System.Windows.Forms.Label();
         secretGroubBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // secretGroubBox
         // 
         secretGroubBox.Controls.Add(this.m_SecretTextBox);
         secretGroubBox.Controls.Add(secretLabel);
         secretGroubBox.Controls.Add(this.m_IssuerTextBox);
         secretGroubBox.Controls.Add(issuerLabel);
         secretGroubBox.Location = new System.Drawing.Point(12,54);
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
         this.m_SecretTextBox.TextChanged += new System.EventHandler(this.OnSecretTextChanged);
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
         // namespaceLabel
         // 
         namespaceLabel.AutoSize = true;
         namespaceLabel.Location = new System.Drawing.Point(9,12);
         namespaceLabel.Name = "namespaceLabel";
         namespaceLabel.Size = new System.Drawing.Size(106,13);
         namespaceLabel.TabIndex = 0;
         namespaceLabel.Text = "Service Namespace:";
         // 
         // m_LogonButton
         // 
         this.m_LogonButton.Enabled = false;
         this.m_LogonButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
         this.m_LogonButton.Location = new System.Drawing.Point(198,147);
         this.m_LogonButton.Name = "m_LogonButton";
         this.m_LogonButton.Size = new System.Drawing.Size(78,23);
         this.m_LogonButton.TabIndex = 7;
         this.m_LogonButton.Text = "Login";
         this.m_LogonButton.UseVisualStyleBackColor = true;
         this.m_LogonButton.Click += new System.EventHandler(this.OnLogon);
         // 
         // m_NamespaceTextbox
         // 
         this.m_NamespaceTextbox.Location = new System.Drawing.Point(12,28);
         this.m_NamespaceTextbox.Name = "m_NamespaceTextbox";
         this.m_NamespaceTextbox.Size = new System.Drawing.Size(180,20);
         this.m_NamespaceTextbox.TabIndex = 1;
         this.m_NamespaceTextbox.TextChanged += new System.EventHandler(this.OnNamespaceTextChanged);
         // 
         // LogonDialog
         // 
         this.AcceptButton = this.m_LogonButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(285,187);
         this.Controls.Add(this.m_LogonButton);
         this.Controls.Add(this.m_NamespaceTextbox);
         this.Controls.Add(namespaceLabel);
         this.Controls.Add(secretGroubBox);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "LogonDialog";
         this.Text = "Logon to the AppFabric Service Bus";
         secretGroubBox.ResumeLayout(false);
         secretGroubBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox m_SecretTextBox;
      private System.Windows.Forms.TextBox m_IssuerTextBox;
      private System.Windows.Forms.Button m_LogonButton;
      private System.Windows.Forms.TextBox m_NamespaceTextbox;

   }
}