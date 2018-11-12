' 让VBS脚本自动在词本中输入一行文字"Hello, welcome to cfan".
'http://blog.csdn.net/guopengzhang/article/details/4905120
Dim WshShell
Set WshShell=WScript.CreateObject("WScript.Shell")
WshShell.Run "notepad"
WScript.Sleep 2000
WshShell.AppActivate " Untitled - Notepad "
WshShell.SendKeys "hello, welcome to cfan"