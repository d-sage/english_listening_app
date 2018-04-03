<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html 
        {
           margin: 0px;
           height: 100%;
           width: 100%;
           background-color: aqua;
        }

        body 
        {
           margin: 0px;
           min-height: 100%;
           width: 100%;
           background-color: aqua;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Label ID="SiteName" runat="server" Text="English Listening APP" style="z-index: 1; left: 330px; top: 5px; position: absolute" Font-Bold="true" Font-Size="72px" Font-Underline="true" ></asp:Label>
        </div>
    
        <asp:Label ID="lblTime" runat="server" style="z-index: 1; left: 15px; top: 100px; position: absolute" ></asp:Label>
        
        <!-- Username box and labels -->
        <asp:TextBox ID="user" runat="server" style="z-index: 1; left: 600px; top: 330px; position: absolute" ></asp:TextBox>
        <asp:Label ID="lbluser" runat="server" Text="Username:" style="z-index: 1; left: 527px; top: 330px; position: absolute"></asp:Label>
        
        <!-- Password box and labels -->        
        <asp:TextBox ID="pass" runat="server" style="z-index: 1; left: 600px; top: 360px; position: absolute"></asp:TextBox>
        <asp:Label ID="lblpass" runat="server" Text="Password:" style="z-index: 1; left: 530px; top: 360px; position: absolute"></asp:Label>
    
        <!-- Submit box and error message -->
        <asp:Button ID="Submit" runat="server" Text="Submit" style="z-index: 1; left: 650px; top: 400px; position: absolute" OnClick="Submit_Click"/>
        <asp:Label ID="errormsg" runat="server" style="z-index: 1; left: 550px; top: 430px; position: absolute"></asp:Label>
        <asp:Label ID="incorrectmsg" runat="server" style="z-index: 1; left: 550px; top: 460px; position: absolute"></asp:Label>
        <asp:Label ID="errormsgDB" runat="server" style="z-index: 1; left: 550px; top: 490px; position: absolute"></asp:Label>

    </form>
</body>
</html>
