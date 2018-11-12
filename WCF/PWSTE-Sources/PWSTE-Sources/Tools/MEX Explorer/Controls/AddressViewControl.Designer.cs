namespace ServiceModelEx
{
   partial class AddressViewControl
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
         System.Windows.Forms.GroupBox uriGroupBox;
         System.Windows.Forms.Label absoluteURILabel;
         System.Windows.Forms.GroupBox addressPropertiesGroup;
         System.Windows.Forms.Label localPathLabel;
         this.m_PortLabel = new System.Windows.Forms.Label();
         this.m_IsLoopbackLabel = new System.Windows.Forms.Label();
         this.m_IsAbsoluteUriLabel = new System.Windows.Forms.Label();
         this.m_HostNameTypeLabel = new System.Windows.Forms.Label();
         this.m_HostLabel = new System.Windows.Forms.Label();
         this.m_DnsSafeHostLabel = new System.Windows.Forms.Label();
         this.m_AuthorityLabel = new System.Windows.Forms.Label();
         this.m_AbsoluteURITextBox = new System.Windows.Forms.TextBox();
         this.m_IsAnonymousLabel = new System.Windows.Forms.Label();
         this.m_IsNoneLabel = new System.Windows.Forms.Label();
         this.m_IdentityLabel = new System.Windows.Forms.Label();
         this.m_LocalPathTextBox = new System.Windows.Forms.TextBox();
         this.m_SchemaLabel = new System.Windows.Forms.Label();
         uriGroupBox = new System.Windows.Forms.GroupBox();
         absoluteURILabel = new System.Windows.Forms.Label();
         addressPropertiesGroup = new System.Windows.Forms.GroupBox();
         localPathLabel = new System.Windows.Forms.Label();
         uriGroupBox.SuspendLayout();
         addressPropertiesGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // uriGroupBox
         // 
         uriGroupBox.Controls.Add(this.m_SchemaLabel);
         uriGroupBox.Controls.Add(this.m_LocalPathTextBox);
         uriGroupBox.Controls.Add(localPathLabel);
         uriGroupBox.Controls.Add(this.m_PortLabel);
         uriGroupBox.Controls.Add(this.m_IsLoopbackLabel);
         uriGroupBox.Controls.Add(this.m_IsAbsoluteUriLabel);
         uriGroupBox.Controls.Add(this.m_HostNameTypeLabel);
         uriGroupBox.Controls.Add(this.m_HostLabel);
         uriGroupBox.Controls.Add(this.m_DnsSafeHostLabel);
         uriGroupBox.Controls.Add(this.m_AuthorityLabel);
         uriGroupBox.Controls.Add(this.m_AbsoluteURITextBox);
         uriGroupBox.Controls.Add(absoluteURILabel);
         uriGroupBox.Location = new System.Drawing.Point(14,119);
         uriGroupBox.Name = "uriGroupBox";
         uriGroupBox.Size = new System.Drawing.Size(355,203);
         uriGroupBox.TabIndex = 0;
         uriGroupBox.TabStop = false;
         uriGroupBox.Text = "URI";
         // 
         // m_PortLabel
         // 
         this.m_PortLabel.AutoSize = true;
         this.m_PortLabel.Location = new System.Drawing.Point(216,155);
         this.m_PortLabel.Name = "m_PortLabel";
         this.m_PortLabel.Size = new System.Drawing.Size(32,13);
         this.m_PortLabel.TabIndex = 11;
         this.m_PortLabel.Text = "Port: ";
         // 
         // m_IsLoopbackLabel
         // 
         this.m_IsLoopbackLabel.AutoSize = true;
         this.m_IsLoopbackLabel.Location = new System.Drawing.Point(216,131);
         this.m_IsLoopbackLabel.Name = "m_IsLoopbackLabel";
         this.m_IsLoopbackLabel.Size = new System.Drawing.Size(61,13);
         this.m_IsLoopbackLabel.TabIndex = 9;
         this.m_IsLoopbackLabel.Text = "Loopback: ";
         // 
         // m_IsAbsoluteUriLabel
         // 
         this.m_IsAbsoluteUriLabel.AutoSize = true;
         this.m_IsAbsoluteUriLabel.Location = new System.Drawing.Point(216,108);
         this.m_IsAbsoluteUriLabel.Name = "m_IsAbsoluteUriLabel";
         this.m_IsAbsoluteUriLabel.Size = new System.Drawing.Size(73,13);
         this.m_IsAbsoluteUriLabel.TabIndex = 8;
         this.m_IsAbsoluteUriLabel.Text = "Absolute URI:";
         // 
         // m_HostNameTypeLabel
         // 
         this.m_HostNameTypeLabel.AutoSize = true;
         this.m_HostNameTypeLabel.Location = new System.Drawing.Point(6,178);
         this.m_HostNameTypeLabel.Name = "m_HostNameTypeLabel";
         this.m_HostNameTypeLabel.Size = new System.Drawing.Size(93,13);
         this.m_HostNameTypeLabel.TabIndex = 7;
         this.m_HostNameTypeLabel.Text = "Host Name Type: ";
         // 
         // m_HostLabel
         // 
         this.m_HostLabel.AutoSize = true;
         this.m_HostLabel.Location = new System.Drawing.Point(6,155);
         this.m_HostLabel.Name = "m_HostLabel";
         this.m_HostLabel.Size = new System.Drawing.Size(35,13);
         this.m_HostLabel.TabIndex = 6;
         this.m_HostLabel.Text = "Host: ";
         // 
         // m_DnsSafeHostLabel
         // 
         this.m_DnsSafeHostLabel.AutoSize = true;
         this.m_DnsSafeHostLabel.Location = new System.Drawing.Point(6,131);
         this.m_DnsSafeHostLabel.Name = "m_DnsSafeHostLabel";
         this.m_DnsSafeHostLabel.Size = new System.Drawing.Size(86,13);
         this.m_DnsSafeHostLabel.TabIndex = 5;
         this.m_DnsSafeHostLabel.Text = "DNS Safe Host: ";
         // 
         // m_AuthorityLabel
         // 
         this.m_AuthorityLabel.AutoSize = true;
         this.m_AuthorityLabel.Location = new System.Drawing.Point(6,108);
         this.m_AuthorityLabel.Name = "m_AuthorityLabel";
         this.m_AuthorityLabel.Size = new System.Drawing.Size(54,13);
         this.m_AuthorityLabel.TabIndex = 4;
         this.m_AuthorityLabel.Text = "Authority: ";
         // 
         // m_AbsoluteURITextBox
         // 
         this.m_AbsoluteURITextBox.Location = new System.Drawing.Point(6,35);
         this.m_AbsoluteURITextBox.Name = "m_AbsoluteURITextBox";
         this.m_AbsoluteURITextBox.Size = new System.Drawing.Size(343,20);
         this.m_AbsoluteURITextBox.TabIndex = 3;
         // 
         // absoluteURILabel
         // 
         absoluteURILabel.AutoSize = true;
         absoluteURILabel.Location = new System.Drawing.Point(3,19);
         absoluteURILabel.Name = "absoluteURILabel";
         absoluteURILabel.Size = new System.Drawing.Size(73,13);
         absoluteURILabel.TabIndex = 2;
         absoluteURILabel.Text = "Absolute URI:";
         // 
         // addressPropertiesGroup
         // 
         addressPropertiesGroup.Controls.Add(this.m_IsAnonymousLabel);
         addressPropertiesGroup.Controls.Add(this.m_IsNoneLabel);
         addressPropertiesGroup.Controls.Add(this.m_IdentityLabel);
         addressPropertiesGroup.Location = new System.Drawing.Point(14,12);
         addressPropertiesGroup.Name = "addressPropertiesGroup";
         addressPropertiesGroup.Size = new System.Drawing.Size(355,101);
         addressPropertiesGroup.TabIndex = 0;
         addressPropertiesGroup.TabStop = false;
         addressPropertiesGroup.Text = "Address Properties";
         // 
         // m_IsAnonymousLabel
         // 
         this.m_IsAnonymousLabel.AutoSize = true;
         this.m_IsAnonymousLabel.Location = new System.Drawing.Point(6,73);
         this.m_IsAnonymousLabel.Name = "m_IsAnonymousLabel";
         this.m_IsAnonymousLabel.Size = new System.Drawing.Size(79,13);
         this.m_IsAnonymousLabel.TabIndex = 2;
         this.m_IsAnonymousLabel.Text = "Is Anonymous: ";
         // 
         // m_IsNoneLabel
         // 
         this.m_IsNoneLabel.AutoSize = true;
         this.m_IsNoneLabel.Location = new System.Drawing.Point(6,49);
         this.m_IsNoneLabel.Name = "m_IsNoneLabel";
         this.m_IsNoneLabel.Size = new System.Drawing.Size(50,13);
         this.m_IsNoneLabel.TabIndex = 1;
         this.m_IsNoneLabel.Text = "Is None: ";
         // 
         // m_IdentityLabel
         // 
         this.m_IdentityLabel.AutoSize = true;
         this.m_IdentityLabel.Location = new System.Drawing.Point(6,27);
         this.m_IdentityLabel.Name = "m_IdentityLabel";
         this.m_IdentityLabel.Size = new System.Drawing.Size(47,13);
         this.m_IdentityLabel.TabIndex = 0;
         this.m_IdentityLabel.Text = "Identity: ";
         // 
         // m_LocalPathTextBox
         // 
         this.m_LocalPathTextBox.Location = new System.Drawing.Point(7,77);
         this.m_LocalPathTextBox.Name = "m_LocalPathTextBox";
         this.m_LocalPathTextBox.Size = new System.Drawing.Size(343,20);
         this.m_LocalPathTextBox.TabIndex = 13;
         // 
         // localPathLabel
         // 
         localPathLabel.AutoSize = true;
         localPathLabel.Location = new System.Drawing.Point(4,61);
         localPathLabel.Name = "localPathLabel";
         localPathLabel.Size = new System.Drawing.Size(61,13);
         localPathLabel.TabIndex = 12;
         localPathLabel.Text = "Local Path:";
         // 
         // m_SchemaLabel
         // 
         this.m_SchemaLabel.AutoSize = true;
         this.m_SchemaLabel.Location = new System.Drawing.Point(216,178);
         this.m_SchemaLabel.Name = "m_SchemaLabel";
         this.m_SchemaLabel.Size = new System.Drawing.Size(52,13);
         this.m_SchemaLabel.TabIndex = 14;
         this.m_SchemaLabel.Text = "Schema: ";
         // 
         // AddressViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(addressPropertiesGroup);
         this.Controls.Add(uriGroupBox);
         this.Name = "AddressViewControl";
         uriGroupBox.ResumeLayout(false);
         uriGroupBox.PerformLayout();
         addressPropertiesGroup.ResumeLayout(false);
         addressPropertiesGroup.PerformLayout();
         this.ResumeLayout(false);

      }
      #endregion

      private System.Windows.Forms.Label m_IdentityLabel;
      private System.Windows.Forms.Label m_IsAnonymousLabel;
      private System.Windows.Forms.Label m_IsNoneLabel;
      private System.Windows.Forms.TextBox m_AbsoluteURITextBox;
      private System.Windows.Forms.Label m_AuthorityLabel;
      private System.Windows.Forms.Label m_DnsSafeHostLabel;
      private System.Windows.Forms.Label m_HostLabel;
      private System.Windows.Forms.Label m_HostNameTypeLabel;
      private System.Windows.Forms.Label m_IsAbsoluteUriLabel;
      private System.Windows.Forms.Label m_IsLoopbackLabel;
      private System.Windows.Forms.Label m_PortLabel;
      private System.Windows.Forms.Label m_SchemaLabel;
      private System.Windows.Forms.TextBox m_LocalPathTextBox;



   }
}
