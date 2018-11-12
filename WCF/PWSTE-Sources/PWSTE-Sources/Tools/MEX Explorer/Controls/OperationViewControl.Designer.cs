namespace ServiceModelEx
{
   partial class OperationViewControl
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
         System.Windows.Forms.GroupBox aspectsGroupBox;
         System.Windows.Forms.GroupBox transactionGroupBox;
         System.Windows.Forms.GroupBox protectionGroupBox;
         System.Windows.Forms.Label knownTypesLabel;
         System.Windows.Forms.Label faultsLable;
         this.m_FormatStyleLabel = new System.Windows.Forms.Label();
         this.m_OneWayLabel = new System.Windows.Forms.Label();
         this.m_MandatoryRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NotAllowedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_AllowedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_EncryptRadioButton = new System.Windows.Forms.RadioButton();
         this.m_SignedRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NonProtectionRadioButton = new System.Windows.Forms.RadioButton();
         this.m_NameLabel = new System.Windows.Forms.Label();
         this.m_KnownTypesListView = new System.Windows.Forms.ListView();
         this.m_FaultsListView = new System.Windows.Forms.ListView();
         aspectsGroupBox = new System.Windows.Forms.GroupBox();
         transactionGroupBox = new System.Windows.Forms.GroupBox();
         protectionGroupBox = new System.Windows.Forms.GroupBox();
         knownTypesLabel = new System.Windows.Forms.Label();
         faultsLable = new System.Windows.Forms.Label();
         aspectsGroupBox.SuspendLayout();
         transactionGroupBox.SuspendLayout();
         protectionGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // aspectsGroupBox
         // 
         aspectsGroupBox.Controls.Add(this.m_FormatStyleLabel);
         aspectsGroupBox.Controls.Add(this.m_OneWayLabel);
         aspectsGroupBox.Controls.Add(transactionGroupBox);
         aspectsGroupBox.Controls.Add(protectionGroupBox);
         aspectsGroupBox.Location = new System.Drawing.Point(13,29);
         aspectsGroupBox.Name = "aspectsGroupBox";
         aspectsGroupBox.Size = new System.Drawing.Size(353,121);
         aspectsGroupBox.TabIndex = 9;
         aspectsGroupBox.TabStop = false;
         aspectsGroupBox.Text = "Aspects";
         // 
         // m_FormatStyleLabel
         // 
         this.m_FormatStyleLabel.AutoSize = true;
         this.m_FormatStyleLabel.Location = new System.Drawing.Point(6,37);
         this.m_FormatStyleLabel.Name = "m_FormatStyleLabel";
         this.m_FormatStyleLabel.Size = new System.Drawing.Size(68,13);
         this.m_FormatStyleLabel.TabIndex = 13;
         this.m_FormatStyleLabel.Text = "Format Style:";
         // 
         // m_OneWayLabel
         // 
         this.m_OneWayLabel.AutoSize = true;
         this.m_OneWayLabel.Location = new System.Drawing.Point(6,16);
         this.m_OneWayLabel.Name = "m_OneWayLabel";
         this.m_OneWayLabel.Size = new System.Drawing.Size(58,13);
         this.m_OneWayLabel.TabIndex = 5;
         this.m_OneWayLabel.Text = "One Way: ";
         // 
         // transactionGroupBox
         // 
         transactionGroupBox.Controls.Add(this.m_MandatoryRadioButton);
         transactionGroupBox.Controls.Add(this.m_NotAllowedRadioButton);
         transactionGroupBox.Controls.Add(this.m_AllowedRadioButton);
         transactionGroupBox.Location = new System.Drawing.Point(118,16);
         transactionGroupBox.Name = "transactionGroupBox";
         transactionGroupBox.Size = new System.Drawing.Size(105,94);
         transactionGroupBox.TabIndex = 2;
         transactionGroupBox.TabStop = false;
         transactionGroupBox.Text = "Transaction Flow";
         // 
         // m_MandatoryRadioButton
         // 
         this.m_MandatoryRadioButton.AutoSize = true;
         this.m_MandatoryRadioButton.Location = new System.Drawing.Point(6,63);
         this.m_MandatoryRadioButton.Name = "m_MandatoryRadioButton";
         this.m_MandatoryRadioButton.Size = new System.Drawing.Size(75,17);
         this.m_MandatoryRadioButton.TabIndex = 0;
         this.m_MandatoryRadioButton.TabStop = true;
         this.m_MandatoryRadioButton.Text = "Mandatory";
         this.m_MandatoryRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_NotAllowedRadioButton
         // 
         this.m_NotAllowedRadioButton.AutoSize = true;
         this.m_NotAllowedRadioButton.Location = new System.Drawing.Point(6,40);
         this.m_NotAllowedRadioButton.Name = "m_NotAllowedRadioButton";
         this.m_NotAllowedRadioButton.Size = new System.Drawing.Size(82,17);
         this.m_NotAllowedRadioButton.TabIndex = 0;
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
         this.m_AllowedRadioButton.TabIndex = 0;
         this.m_AllowedRadioButton.TabStop = true;
         this.m_AllowedRadioButton.Text = "Allowed";
         this.m_AllowedRadioButton.UseVisualStyleBackColor = true;
         // 
         // protectionGroupBox
         // 
         protectionGroupBox.Controls.Add(this.m_EncryptRadioButton);
         protectionGroupBox.Controls.Add(this.m_SignedRadioButton);
         protectionGroupBox.Controls.Add(this.m_NonProtectionRadioButton);
         protectionGroupBox.Location = new System.Drawing.Point(229,16);
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
         // knownTypesLabel
         // 
         knownTypesLabel.AutoSize = true;
         knownTypesLabel.Location = new System.Drawing.Point(19,164);
         knownTypesLabel.Name = "knownTypesLabel";
         knownTypesLabel.Size = new System.Drawing.Size(75,13);
         knownTypesLabel.TabIndex = 6;
         knownTypesLabel.Text = "Known Types:";
         // 
         // faultsLable
         // 
         faultsLable.AutoSize = true;
         faultsLable.Location = new System.Drawing.Point(195,164);
         faultsLable.Name = "faultsLable";
         faultsLable.Size = new System.Drawing.Size(38,13);
         faultsLable.TabIndex = 11;
         faultsLable.Text = "Faults:";
         // 
         // m_NameLabel
         // 
         this.m_NameLabel.AutoSize = true;
         this.m_NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",10F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
         this.m_NameLabel.Location = new System.Drawing.Point(10,9);
         this.m_NameLabel.Name = "m_NameLabel";
         this.m_NameLabel.Size = new System.Drawing.Size(59,17);
         this.m_NameLabel.TabIndex = 7;
         this.m_NameLabel.Text = "Name: ";
         // 
         // m_KnownTypesListView
         // 
         this.m_KnownTypesListView.FullRowSelect = true;
         this.m_KnownTypesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.m_KnownTypesListView.Location = new System.Drawing.Point(22,180);
         this.m_KnownTypesListView.MultiSelect = false;
         this.m_KnownTypesListView.Name = "m_KnownTypesListView";
         this.m_KnownTypesListView.ShowGroups = false;
         this.m_KnownTypesListView.Size = new System.Drawing.Size(160,136);
         this.m_KnownTypesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
         this.m_KnownTypesListView.TabIndex = 10;
         this.m_KnownTypesListView.UseCompatibleStateImageBehavior = false;
         this.m_KnownTypesListView.View = System.Windows.Forms.View.SmallIcon;
         // 
         // m_FaultsListView
         // 
         this.m_FaultsListView.FullRowSelect = true;
         this.m_FaultsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.m_FaultsListView.Location = new System.Drawing.Point(198,180);
         this.m_FaultsListView.MultiSelect = false;
         this.m_FaultsListView.Name = "m_FaultsListView";
         this.m_FaultsListView.ShowGroups = false;
         this.m_FaultsListView.Size = new System.Drawing.Size(160,136);
         this.m_FaultsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
         this.m_FaultsListView.TabIndex = 12;
         this.m_FaultsListView.UseCompatibleStateImageBehavior = false;
         this.m_FaultsListView.View = System.Windows.Forms.View.SmallIcon;
         // 
         // OperationViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(this.m_FaultsListView);
         this.Controls.Add(faultsLable);
         this.Controls.Add(this.m_KnownTypesListView);
         this.Controls.Add(knownTypesLabel);
         this.Controls.Add(aspectsGroupBox);
         this.Controls.Add(this.m_NameLabel);
         this.Name = "OperationViewControl";
         aspectsGroupBox.ResumeLayout(false);
         aspectsGroupBox.PerformLayout();
         transactionGroupBox.ResumeLayout(false);
         transactionGroupBox.PerformLayout();
         protectionGroupBox.ResumeLayout(false);
         protectionGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion

      private System.Windows.Forms.Label m_OneWayLabel;
      private System.Windows.Forms.RadioButton m_EncryptRadioButton;
      private System.Windows.Forms.RadioButton m_SignedRadioButton;
      private System.Windows.Forms.RadioButton m_NonProtectionRadioButton;
      private System.Windows.Forms.Label m_NameLabel;
      private System.Windows.Forms.ListView m_KnownTypesListView;
      private System.Windows.Forms.ListView m_FaultsListView;
      private System.Windows.Forms.RadioButton m_MandatoryRadioButton;
      private System.Windows.Forms.RadioButton m_NotAllowedRadioButton;
      private System.Windows.Forms.RadioButton m_AllowedRadioButton;
      private System.Windows.Forms.Label m_FormatStyleLabel;


   }
}
