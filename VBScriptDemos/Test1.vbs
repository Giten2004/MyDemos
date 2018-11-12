Set Sh = CreateObject("WScript.Shell")

strComputer = "."
Set objWMIService = GetObject("winmgmts:" & "{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
Set colProcessList = objWMIService.ExecQuery("Select * from Win32_Process Where Name = 'Notepad.exe'")

'-----------------------------------------------------------------------------------------------------------------------------------------
'                           THIS DEFINES THE PATH AS TO WHERE THE SCENARIOS ARE TO BE SAVED ON THE NETWORK
'-----------------------------------------------------------------------------------------------------------------------------------------

destPathDir = "\\aus\share\departments\Risk Management\Basel\TempBasel\Temp\"
PickUpName = "\\aus\share\departments\Risk Management\Basel\TempBasel\"
'-----------------------------------------------------------------------------------------------------------------------------------------
'                           THIS IS A LIST OF THE SCENARIOS THAT ARE TO BE COPIED FROM REACTOR INTO A NOTEPAD
'                           BEFORE BEING SAVED ONTO THE NETWORK
'-----------------------------------------------------------------------------------------------------------------------------------------

WriteFilesAndCopy


Sub WriteFilesAndCopy()


	CopyWindowToTextfile "40XJO", ".scn", destPathDir
	CopyToReadLocation "40XJO", destPathDir, PickUpName

	CopyWindowToTextfile "FI", ".pos", destPathDir
	CopyToReadLocation "FI", destPathDir, PickUpName

	CopyWindowToTextfile "40%NKY_3", ".scn", destPathDir
	CopyToReadLocation "40%NKY_3", destPathDir, PickUpName	

	CopyWindowToTextfile "SYD - ASX Single Stock", ".scn", destPathDir
	CopyToReadLocation "SYD - ASX Single Stock", destPathDir, PickUpName

	CopyWindowToTextfile "40%HHI_T1", ".scn", destPathDir
	CopyToReadLocation "40%HHI_T1", destPathDir, PickUpName

	CopyWindowToTextfile "SYD - SinglesVegaMonth", ".pos", destPathDir
	CopyToReadLocation "SYD - SinglesVegaMonth", destPathDir, PickUpName

	CopyWindowToTextfile "SYD - LepoPremium", ".pos", destPathDir
	CopyToReadLocation "SYD - LepoPremium", destPathDir, PickUpName
	
	CopyWindowToTextfile "SYD - CCCashDelta", ".pos", destPathDir
	CopyToReadLocation "SYD - CCCashDelta", destPathDir, PickUpName

	CopyWindowToTextfile "SYD -SinglesSummary", ".pos", destPathDir
	CopyToReadLocation "SYD -SinglesSummary", destPathDir, PickUpName
	
	CopyWindowToTextfile "SYD-NKY3Premium", ".pos", destPathDir
	CopyToReadLocation "SYD-NKY3Premium", destPathDir, PickUpName
	
	CopyWindowToTextfile "RollMonitor", ".pos", destPathDir
	CopyToReadLocation "RollMonitor", destPathDir, PickUpName

	CopyWindowToTextfile "NKY3Vanna", ".scn", destPathDir
	CopyToReadLocation "NKY3Vanna", destPathDir, PickUpName

	CopyWindowToTextfile "IndexSummary", ".pos", destPathDir
	CopyToReadLocation "IndexSummary", destPathDir, PickUpName
	
	CopyWindowToTextfile "SYD - CCCashDeltaExFund3", ".pos", destPathDir
	CopyToReadLocation "SYD - CCCashDeltaExFund3", destPathDir, PickUpName
	
	CopyWindowToTextfile "CBA_Fund3", ".pos", destPathDir
	CopyToReadLocation "CBA_Fund3", destPathDir, PickUpName

	CopyWindowToTextfile "LEPO_PREMO_ XJO", ".pos", destPathDir
	CopyToReadLocation "LEPO_PREMO_ XJO", destPathDir, PickUpName

	CopyWindowToTextfile "40%SingleStock1", ".scn", destPathDir
	CopyToReadLocation "40%SingleStock1", destPathDir, PickUpName

	CopyWindowToTextfile "40%HSI1", ".scn", destPathDir
	CopyToReadLocation "40%HSI1", destPathDir, PickUpName

	
End Sub

Sub CopyToReadLocation (strFile, strSourceFolder, strDestFolder)

	Set filesys = CreateObject("Scripting.FileSystemObject")	
	filesys.CopyFile strSourceFolder & strFile & ".txt", strDestFolder & strFile & ".txt", true
	
End Sub




'============================================================================================================================================

'           THIS MACRO COPIES EACH OF THE NAMED REACTOR WINDOWS INTO A NOTEPAD DOCUMENT AND SAVES IT ONTO THE SHARED DRIVE

'============================================================================================================================================

Sub CopyWindowToTextfile(winname, wintype, destPathDir)
    
    Dim fso, MyFile, MyFile1
    Set fso = CreateObject("Scripting.FileSystemObject")
    
    	'IF THERE IS AN ERROR, THE CODE WILL (OR SHOULD) IGNORE IT AND CONTINUE
	On Error Resume Next
    
    
    Success = False
    Do Until Success = True
        Success = Sh.AppActivate(winname & wintype)
        WScript.sleep 1300
        If Success = "False" Then
        MsgBox "Window " & winname & wintype & " doesnt exist"
        'Exit Sub
        End If
    Loop
    
    WScript.sleep 500
	' ALT + E， then enter
	' copy content
    Sh.SendKeys "%E{ENTER}"
    WScript.sleep 750
    
    Set Sh = CreateObject("WScript.Shell")
	
    ' https://stackoverflow.com/questions/19696308/how-can-i-use-clipboard-in-vbscript
    Set objHTML = CreateObject("htmlfile")
    ClipText = objHTML.ParentWindow.ClipboardData.GetData("text")
    WScript.sleep 750
    
    Set MyFile1 = fso.CreateTextFile(destPathDir & winname & ".txt", True)
    MyFile1.WriteLine (ClipText)
    MyFile1.Close
    WScript.sleep 1200

	'TO MINIMISE THE RISK OF AN ERROR, THIS WILL TERMINATE ANY OPEN SESSIONS OF
	'NOTEPAD BEFORE CONTINUING

	For Each objProcess in colProcessList 
     		objProcess.Terminate() 
	Next 

    
End Sub
