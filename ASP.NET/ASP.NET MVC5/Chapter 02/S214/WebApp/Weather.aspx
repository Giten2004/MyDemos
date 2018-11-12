<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Weather.aspx.cs" Inherits="WebApp.Weather" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>自定义Route</title>
    <link rel="stylesheet" href="/Content/bootstrap.css" /> 
    <link rel="stylesheet" href="/Content/site.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="table table-bordered">
            <tr>
                <td>Route:</td>
                <td><%=RouteData.Route != null? RouteData.Route.GetType().FullName:"" %></td>
            </tr>
            <tr>
                <td>RouteHandler:</td>
                <td><%=RouteData.RouteHandler != null? RouteData.RouteHandler.GetType().FullName:"" %></td>
            </tr>
            <tr>
                <td>Values:</td>
                <td>
                    <ul>
                        <%foreach (var variable in RouteData.Values)
                          {%>
                        <li>
                            <%=variable.Key%>=<%=variable.Value%></li>
                        <% }%>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>DataTokens:</td>
                <td>
                    <ul>
                        <%foreach (var variable in RouteData.DataTokens)
                          {%>
                        <li>
                            <%=variable.Key%>=<%=variable.Value%></li>
                        <% }%>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>Generated Url:</td>
                <td>
                    <%=GenerateUrl("0512",3)%>
                </td>
            </tr>
        </table>
    </div>
</form>

</body>
</html>
