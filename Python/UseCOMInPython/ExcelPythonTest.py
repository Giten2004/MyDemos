import win32com.client
o = win32com.client.Dispatch("Excel.Application")
o.Visible = 1
o.Workbooks.Add()
#And we will see the word "Hello" appear in the top cell.
o.Cells(1,1).Value = "Hello" 