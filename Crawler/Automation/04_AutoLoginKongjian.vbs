Set ie=WScript.CreateObject("InternetExplorer.Application") 
ie.visible=true 
ie.navigate "http://passport.baidu.com/?login" 
Do 
Wscript.Sleep 200 
Loop Until ie.ReadyState=4 
ie.document.getElementById("username").value="账号" 
ie.document.getElementById("password").value="密码" 
ie.document.getElementById("mem_pass").checked=true  
Wscript.Sleep 200 
dl=ie.document.getElementsBytagname("form") 
Wscript.Sleep 200 
dl.submit