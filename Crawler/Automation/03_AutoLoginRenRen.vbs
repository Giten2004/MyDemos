Dim a 
Set a=Wscript.CreateObject("Wscript.Shell") 
a.run   "http://www.renren.com/GLogin.do" 
Wscript.Sleep   3000 
a.SendKeys "账号"  '光标位置在账号框
Wscript.Sleep 100 
a.SendKeys "{Tab}" 
a.SendKeys "密码" 
a.SendKeys "{Tab}" 
a.SendKeys "{Tab}" 
a.SendKeys "{Enter}" 
Wscript.Sleep   300 
a.SendKeys "{Enter}" 