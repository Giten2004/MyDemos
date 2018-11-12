namespace ServiceModelEx
{
   partial class EndpointViewControl
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
         System.Windows.Forms.GroupBox listeningGroup;
         System.Windows.Forms.GroupBox listeningModeGroup;
         System.Windows.Forms.Label uriLabel;
         this.m_UniqueRadioButton = new System.Windows.Forms.RadioButton();
         this.m_ExplicitRadioButton = new System.Windows.Forms.RadioButton();
         this.m_ListeningTextBox = new System.Windows.Forms.TextBox();
         this.m_NameLabel = new System.Windows.Forms.Label();
         listeningGroup = new System.Windows.Forms.GroupBox();
         listeningModeGroup = new System.Windows.Forms.GroupBox();
         uriLabel = new System.Windows.Forms.Label();
         listeningGroup.SuspendLayout();
         listeningModeGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // listeningGroup
         // 
         listeningGroup.Controls.Add(listeningModeGroup);
         listeningGroup.Controls.Add(this.m_ListeningTextBox);
         listeningGroup.Controls.Add(uriLabel);
         listeningGroup.Location = new System.Drawing.Point(15,39);
         listeningGroup.Name = "listeningGroup";
         listeningGroup.Size = new System.Drawing.Size(353,150);
         listeningGroup.TabIndex = 2;
         listeningGroup.TabStop = false;
         listeningGroup.Text = "Listening";
         // 
         // listeningModeGroup
         // 
         listeningModeGroup.Controls.Add(this.m_UniqueRadioButton);
         listeningModeGroup.Controls.Add(this.m_ExplicitRadioButton);
         listeningModeGroup.Location = new System.Drawing.Point(8,68);
         listeningModeGroup.Name = "listeningModeGroup";
         listeningModeGroup.Size = new System.Drawing.Size(114,73);
         listeningModeGroup.TabIndex = 2;
         listeningModeGroup.TabStop = false;
         listeningModeGroup.Text = "Listening Mode";
         // 
         // m_UniqueRadioButton
         // 
         this.m_UniqueRadioButton.AutoSize = true;
         this.m_UniqueRadioButton.Location = new System.Drawing.Point(5,43);
         this.m_UniqueRadioButton.Name = "m_UniqueRadioButton";
         this.m_UniqueRadioButton.Size = new System.Drawing.Size(59,17);
         this.m_UniqueRadioButton.TabIndex = 1;
         this.m_UniqueRadioButton.TabStop = true;
         this.m_UniqueRadioButton.Text = "Unique";
         this.m_UniqueRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_ExplicitRadioButton
         // 
         this.m_ExplicitRadioButton.AutoSize = true;
         this.m_ExplicitRadioButton.Location = new System.Drawing.Point(5,20);
         this.m_ExplicitRadioButton.Name = "m_ExplicitRadioButton";
         this.m_ExplicitRadioButton.Size = new System.Drawing.Size(58,17);
         this.m_ExplicitRadioButton.TabIndex = 0;
         this.m_ExplicitRadioButton.TabStop = true;
         this.m_ExplicitRadioButton.Text = "Explicit";
         this.m_ExplicitRadioButton.UseVisualStyleBackColor = true;
         // 
         // m_ListeningTextBox
         // 
         this.m_ListeningTextBox.Location = new System.Drawing.Point(8,32);
         this.m_ListeningTextBox.Name = "m_ListeningTextBox";
         this.m_ListeningTextBox.Size = new System.Drawing.Size(338,20);
         this.m_ListeningTextBox.TabIndex = 1;
         // 
         // uriLabel
         // 
         uriLabel.AutoSize = true;
         uriLabel.Location = new System.Drawing.Point(5,16);
         uriLabel.Name = "uriLabel";
         uriLabel.Size = new System.Drawing.Size(80,13);
         uriLabel.TabIndex = 0;
         uriLabel.Text = "Listenning URI:";
         // 
         // m_NameLabel
         // 
         this.m_NameLabel.AutoSize = true;
         this.m_NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",10F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
         this.m_NameLabel.Location = new System.Drawing.Point(12,9);
         this.m_NameLabel.Name = "m_NameLabel";
         this.m_NameLabel.Size = new System.Drawing.Size(59,17);
         this.m_NameLabel.TabIndex = 1;
         this.m_NameLabel.Text = "Name: ";
         // 
         // EndpointViewControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(listeningGroup);
         this.Controls.Add(this.m_NameLabel);
         this.Name = "EndpointViewControl";
         listeningGroup.ResumeLayout(false);
         listeningGroup.PerformLayout();
         listeningModeGroup.ResumeLayout(false);
         listeningModeGroup.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion

      private System.Windows.Forms.Label m_NameLabel;
      private System.Windows.Forms.RadioButton m_UniqueRadioButton;
      private System.Windows.Forms.RadioButton m_ExplicitRadioButton;
      private System.Windows.Forms.TextBox m_ListeningTextBox;

   }
}
