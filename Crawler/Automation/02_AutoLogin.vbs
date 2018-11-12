on error resume next
url = " http://www.bathome.net/logging.php?action=login"
username = "username"
password = "password"
set ie = CreateObject("InternetExplorer.Application")
ie.visible = true
ie.Navigate url,4
do until 4=ie.readyState
    WScript.sleep 200
    waittime  = waittime + 200
    if waittime > 15000 then exit do
loop
'WScript.echo waittime
if 4<>ie.readyState then
    ie.quit
    WScript.quit
end if
set dom = ie.document
set form = dom.getElementById("loginform")
form.all("username").value = username
form.all("password").value = password
form.all("cookietime").checked = true
form.all("loginsubmit").click()