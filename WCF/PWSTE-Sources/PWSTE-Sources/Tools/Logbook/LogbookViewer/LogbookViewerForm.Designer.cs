namespace ServiceModelEx
{
   partial class LogbookViewerForm
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
         this.components = new System.ComponentModel.Container();
         this.m_EntriesGrid = new System.Windows.Forms.DataGridView();
         this.m_EntriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.clearButton = new System.Windows.Forms.Button();
         this.refreshButton = new System.Windows.Forms.Button();
         this.m_MachineNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_HostNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_AssemblyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_TypeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_MemberAccessedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_FileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_LineNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_DateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_TimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_EventDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_ExceptionNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.m_ExceptionMessageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         ((System.ComponentModel.ISupportInitialize)(this.m_EntriesGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.m_EntriesBindingSource)).BeginInit();
         this.SuspendLayout();
         // 
         // m_EntriesGrid
         // 
         this.m_EntriesGrid.AutoGenerateColumns = false;
         this.m_EntriesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.m_EntriesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_MachineNameDataGridViewTextBoxColumn,
            this.m_HostNameDataGridViewTextBoxColumn,
            this.m_AssemblyNameDataGridViewTextBoxColumn,
            this.m_TypeNameDataGridViewTextBoxColumn,
            this.m_MemberAccessedDataGridViewTextBoxColumn,
            this.m_FileNameDataGridViewTextBoxColumn,
            this.m_LineNumberDataGridViewTextBoxColumn,
            this.m_DateDataGridViewTextBoxColumn,
            this.m_TimeDataGridViewTextBoxColumn,
            this.m_EventDataGridViewTextBoxColumn,
            this.m_ExceptionNameDataGridViewTextBoxColumn,
            this.m_ExceptionMessageDataGridViewTextBoxColumn});
         this.m_EntriesGrid.DataSource = this.m_EntriesBindingSource;
         this.m_EntriesGrid.Location = new System.Drawing.Point(12,12);
         this.m_EntriesGrid.Name = "m_EntriesGrid";
         this.m_EntriesGrid.Size = new System.Drawing.Size(904,207);
         this.m_EntriesGrid.TabIndex = 0;
         // 
         // m_EntriesBindingSource
         // 
         this.m_EntriesBindingSource.DataSource = typeof(ServiceModelEx.LogbookEntry[]);
         // 
         // clearButton
         // 
         this.clearButton.Location = new System.Drawing.Point(93,238);
         this.clearButton.Name = "clearButton";
         this.clearButton.Size = new System.Drawing.Size(75,23);
         this.clearButton.TabIndex = 1;
         this.clearButton.Text = "Clear";
         this.clearButton.UseVisualStyleBackColor = true;
         this.clearButton.Click += new System.EventHandler(this.OnClear);
         // 
         // refreshButton
         // 
         this.refreshButton.Location = new System.Drawing.Point(12,238);
         this.refreshButton.Name = "refreshButton";
         this.refreshButton.Size = new System.Drawing.Size(75,23);
         this.refreshButton.TabIndex = 2;
         this.refreshButton.Text = "Refresh";
         this.refreshButton.UseVisualStyleBackColor = true;
         this.refreshButton.Click += new System.EventHandler(this.OnReload);
         // 
         // m_MachineNameDataGridViewTextBoxColumn
         // 
         this.m_MachineNameDataGridViewTextBoxColumn.DataPropertyName = "MachineName";
         this.m_MachineNameDataGridViewTextBoxColumn.HeaderText = "Machine";
         this.m_MachineNameDataGridViewTextBoxColumn.Name = "m_MachineNameDataGridViewTextBoxColumn";
         // 
         // m_HostNameDataGridViewTextBoxColumn
         // 
         this.m_HostNameDataGridViewTextBoxColumn.DataPropertyName = "HostName";
         this.m_HostNameDataGridViewTextBoxColumn.HeaderText = "Host";
         this.m_HostNameDataGridViewTextBoxColumn.Name = "m_HostNameDataGridViewTextBoxColumn";
         // 
         // m_AssemblyNameDataGridViewTextBoxColumn
         // 
         this.m_AssemblyNameDataGridViewTextBoxColumn.DataPropertyName = "AssemblyName";
         this.m_AssemblyNameDataGridViewTextBoxColumn.HeaderText = "Assembly";
         this.m_AssemblyNameDataGridViewTextBoxColumn.Name = "m_AssemblyNameDataGridViewTextBoxColumn";
         // 
         // m_TypeNameDataGridViewTextBoxColumn
         // 
         this.m_TypeNameDataGridViewTextBoxColumn.DataPropertyName = "TypeName";
         this.m_TypeNameDataGridViewTextBoxColumn.HeaderText = "Type";
         this.m_TypeNameDataGridViewTextBoxColumn.Name = "m_TypeNameDataGridViewTextBoxColumn";
         // 
         // m_MemberAccessedDataGridViewTextBoxColumn
         // 
         this.m_MemberAccessedDataGridViewTextBoxColumn.DataPropertyName = "MemberAccessed";
         this.m_MemberAccessedDataGridViewTextBoxColumn.HeaderText = "Member Accessed";
         this.m_MemberAccessedDataGridViewTextBoxColumn.Name = "m_MemberAccessedDataGridViewTextBoxColumn";
         // 
         // m_FileNameDataGridViewTextBoxColumn
         // 
         this.m_FileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
         this.m_FileNameDataGridViewTextBoxColumn.HeaderText = "File Name";
         this.m_FileNameDataGridViewTextBoxColumn.Name = "m_FileNameDataGridViewTextBoxColumn";
         // 
         // m_LineNumberDataGridViewTextBoxColumn
         // 
         this.m_LineNumberDataGridViewTextBoxColumn.DataPropertyName = "LineNumber";
         this.m_LineNumberDataGridViewTextBoxColumn.HeaderText = "Line Number";
         this.m_LineNumberDataGridViewTextBoxColumn.Name = "m_LineNumberDataGridViewTextBoxColumn";
         // 
         // m_DateDataGridViewTextBoxColumn
         // 
         this.m_DateDataGridViewTextBoxColumn.DataPropertyName = "Date";
         this.m_DateDataGridViewTextBoxColumn.HeaderText = "Date";
         this.m_DateDataGridViewTextBoxColumn.Name = "m_DateDataGridViewTextBoxColumn";
         // 
         // m_TimeDataGridViewTextBoxColumn
         // 
         this.m_TimeDataGridViewTextBoxColumn.DataPropertyName = "Time";
         this.m_TimeDataGridViewTextBoxColumn.HeaderText = "Time";
         this.m_TimeDataGridViewTextBoxColumn.Name = "m_TimeDataGridViewTextBoxColumn";
         // 
         // m_EventDataGridViewTextBoxColumn
         // 
         this.m_EventDataGridViewTextBoxColumn.DataPropertyName = "Event";
         this.m_EventDataGridViewTextBoxColumn.HeaderText = "Event";
         this.m_EventDataGridViewTextBoxColumn.Name = "m_EventDataGridViewTextBoxColumn";
         // 
         // m_ExceptionNameDataGridViewTextBoxColumn
         // 
         this.m_ExceptionNameDataGridViewTextBoxColumn.DataPropertyName = "ExceptionName";
         this.m_ExceptionNameDataGridViewTextBoxColumn.HeaderText = "Exception";
         this.m_ExceptionNameDataGridViewTextBoxColumn.Name = "m_ExceptionNameDataGridViewTextBoxColumn";
         // 
         // m_ExceptionMessageDataGridViewTextBoxColumn
         // 
         this.m_ExceptionMessageDataGridViewTextBoxColumn.DataPropertyName = "ExceptionMessage";
         this.m_ExceptionMessageDataGridViewTextBoxColumn.HeaderText = "Exception Message";
         this.m_ExceptionMessageDataGridViewTextBoxColumn.Name = "m_ExceptionMessageDataGridViewTextBoxColumn";
          // 
         // LogbookViewerForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(928,273);
         this.Controls.Add(this.refreshButton);
         this.Controls.Add(this.clearButton);
         this.Controls.Add(this.m_EntriesGrid);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "LogbookViewerForm";
         this.Text = "Logbook Viewer";
         this.Load += new System.EventHandler(this.OnReload);
         ((System.ComponentModel.ISupportInitialize)(this.m_EntriesGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.m_EntriesBindingSource)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView m_EntriesGrid;
      private System.Windows.Forms.Button clearButton;
      private System.Windows.Forms.Button refreshButton;
      private System.Windows.Forms.BindingSource m_EntriesBindingSource;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_MachineNameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_HostNameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_AssemblyNameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_TypeNameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_MemberAccessedDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_FileNameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_LineNumberDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_DateDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_TimeDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_EventDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_ExceptionNameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn m_ExceptionMessageDataGridViewTextBoxColumn;
   }
}

