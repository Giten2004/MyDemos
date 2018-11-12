using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ServiceModelEx;

namespace ServiceModelEx
{
   public partial class LogbookViewerForm : Form
   {
      public LogbookViewerForm()
      {
         InitializeComponent();
      }
      static void ResizeGrid(DataGridView grid)
      {
         for (int i = 0;i < grid.ColumnCount;i++)
         {
            grid.AutoResizeColumn(i,DataGridViewAutoSizeColumnMode.AllCells);
         }
      }
      void OnReload(object sender,EventArgs e)
      {
         LogbookManagerClient proxy = new LogbookManagerClient("LogbookTCP");
         LogbookEntry[] entries = proxy.GetEntries();
         proxy.Close();

         m_EntriesBindingSource.DataSource = entries;

         ResizeGrid(m_EntriesGrid);
      }

      void OnClear(object sender,EventArgs e)
      {
         LogbookManagerClient proxy = new LogbookManagerClient();
         proxy.Clear();
         proxy.Close();

         OnReload(this,e);
      }
   }
}