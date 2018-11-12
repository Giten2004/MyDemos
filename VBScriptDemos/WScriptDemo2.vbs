' https://msdn.microsoft.com/en-us/subscriptions/ateytk4a(v=vs.84).aspx

Dim WshShell, oExec
Set WshShell = CreateObject("WScript.Shell")

Set oExec = WshShell.Exec("notepad")
WScript.sleep 1300

    Success = False
    Do Until Success = True
        Success = WshShell.AppActivate("Untitled - Notepad")
        WScript.sleep 1300
        If Success = "False" Then
        MsgBox "Window Untitled doesnt exist"
        'Exit Sub
        End If
    Loop

    Set objHTML = CreateObject("htmlfile")
    ClipText = objHTML.ParentWindow.ClipboardData.GetData("text")
    WScript.sleep 750
WScript.Echo ClipText
	
WshShell.SendKeys "+(ec)"
WshShell.SendKeys "%E{ENTER}"


Do While oExec.Status = 0
     WScript.Sleep 100
Loop

WScript.Echo oExec.Status