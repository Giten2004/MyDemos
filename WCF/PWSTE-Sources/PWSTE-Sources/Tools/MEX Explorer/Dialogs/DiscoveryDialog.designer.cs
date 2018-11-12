namespace ServiceModelEx
{
   partial class DiscoveryDialog
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
         System.Windows.Forms.GroupBox announcementsGroupBox;
         System.Windows.Forms.Label label1;
         System.Windows.Forms.Label discoveryLabel;
         System.Windows.Forms.Label namespaceLabel;
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscoveryDialog));
         this.m_AnnouncementsEnabledCheckbox = new System.Windows.Forms.CheckBox();
         this.m_AnnouncementsTextBox = new System.Windows.Forms.TextBox();
         this.m_DoneButton = new System.Windows.Forms.Button();
         this.m_DiscoveryTextbox = new System.Windows.Forms.TextBox();
         this.m_NamespaceTextbox = new System.Windows.Forms.TextBox();
         announcementsGroupBox = new System.Windows.Forms.GroupBox();
         label1 = new System.Windows.Forms.Label();
         discoveryLabel = new System.Windows.Forms.Label();
         namespaceLabel = new System.Windows.Forms.Label();
         announcementsGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // announcementsGroupBox
         // 
         announcementsGroupBox.Controls.Add(this.m_AnnouncementsEnabledCheckbox);
         announcementsGroupBox.Controls.Add(this.m_AnnouncementsTextBox);
         announcementsGroupBox.Controls.Add(label1);
         announcementsGroupBox.Location = new System.Drawing.Point(15,91);
         announcementsGroupBox.Name = "announcementsGroupBox";
         announcementsGroupBox.Size = new System.Drawing.Size(180,91);
         announcementsGroupBox.TabIndex = 8;
         announcementsGroupBox.TabStop = false;
         announcementsGroupBox.Text = "Availability Announcements";
         // 
         // m_AnnouncementsEnabledCheckbox
         // 
         this.m_AnnouncementsEnabledCheckbox.AutoSize = true;
         this.m_AnnouncementsEnabledCheckbox.Location = new System.Drawing.Point(9,19);
         this.m_AnnouncementsEnabledCheckbox.Name = "m_AnnouncementsEnabledCheckbox";
         this.m_AnnouncementsEnabledCheckbox.Size = new System.Drawing.Size(59,17);
         this.m_AnnouncementsEnabledCheckbox.TabIndex = 2;
         this.m_AnnouncementsEnabledCheckbox.Text = "Enable";
         this.m_AnnouncementsEnabledCheckbox.UseVisualStyleBackColor = true;
         this.m_AnnouncementsEnabledCheckbox.CheckedChanged += new System.EventHandler(this.OnEnableChanged);
         // 
         // m_AnnouncementsTextBox
         // 
         this.m_AnnouncementsTextBox.Location = new System.Drawing.Point(9,55);
         this.m_AnnouncementsTextBox.Name = "m_AnnouncementsTextBox";
         this.m_AnnouncementsTextBox.Size = new System.Drawing.Size(152,20);
         this.m_AnnouncementsTextBox.TabIndex = 1;
         // 
         // label1
         // 
         label1.AutoSize = true;
         label1.Location = new System.Drawing.Point(6,39);
         label1.Name = "label1";
         label1.Size = new System.Drawing.Size(112,13);
         label1.TabIndex = 0;
         label1.Text = "Announcements Path:";
         // 
         // discoveryLabel
         // 
         discoveryLabel.AutoSize = true;
         discoveryLabel.Location = new System.Drawing.Point(12,49);
         discoveryLabel.Name = "discoveryLabel";
         discoveryLabel.Size = new System.Drawing.Size(82,13);
         discoveryLabel.TabIndex = 0;
         discoveryLabel.Text = "Discovery Path:";
         // 
         // namespaceLabel
         // 
         namespaceLabel.AutoSize = true;
         namespaceLabel.Location = new System.Drawing.Point(9,5);
         namespaceLabel.Name = "namespaceLabel";
         namespaceLabel.Size = new System.Drawing.Size(106,13);
         namespaceLabel.TabIndex = 9;
         namespaceLabel.Text = "Service Namespace:";
         // 
         // m_DoneButton
         // 
         this.m_DoneButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
         this.m_DoneButton.Location = new System.Drawing.Point(227,159);
         this.m_DoneButton.Name = "m_DoneButton";
         this.m_DoneButton.Size = new System.Drawing.Size(78,23);
         this.m_DoneButton.TabIndex = 7;
         this.m_DoneButton.Text = "Done";
         this.m_DoneButton.UseVisualStyleBackColor = true;
         this.m_DoneButton.Click += new System.EventHandler(this.OnDone);
         // 
         // m_DiscoveryTextbox
         // 
         this.m_DiscoveryTextbox.Location = new System.Drawing.Point(15,65);
         this.m_DiscoveryTextbox.Name = "m_DiscoveryTextbox";
         this.m_DiscoveryTextbox.Size = new System.Drawing.Size(180,20);
         this.m_DiscoveryTextbox.TabIndex = 1;
         // 
         // m_NamespaceTextbox
         // 
         this.m_NamespaceTextbox.Location = new System.Drawing.Point(12,21);
         this.m_NamespaceTextbox.Name = "m_NamespaceTextbox";
         this.m_NamespaceTextbox.Size = new System.Drawing.Size(180,20);
         this.m_NamespaceTextbox.TabIndex = 1;
         this.m_NamespaceTextbox.TextChanged += new System.EventHandler(this.OnNamespaceTextChanged);
         // 
         // DiscoveryDialog
         // 
         this.AcceptButton = this.m_DoneButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(314,192);
         this.Controls.Add(this.m_NamespaceTextbox);
         this.Controls.Add(namespaceLabel);
         this.Controls.Add(announcementsGroupBox);
         this.Controls.Add(this.m_DoneButton);
         this.Controls.Add(this.m_DiscoveryTextbox);
         this.Controls.Add(discoveryLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "DiscoveryDialog";
         this.Text = "Configure AppFabric Service Bus Discovery";
         announcementsGroupBox.ResumeLayout(false);
         announcementsGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox m_DiscoveryTextbox;
      private System.Windows.Forms.TextBox m_AnnouncementsTextBox;
      private System.Windows.Forms.CheckBox m_AnnouncementsEnabledCheckbox;
      private System.Windows.Forms.TextBox m_NamespaceTextbox;
      private System.Windows.Forms.Button m_DoneButton;

   }
}