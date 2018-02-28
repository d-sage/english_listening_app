<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #form1 {
            height: 617px;
            background-color: aqua;
            width: 1305px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Label ID="Label1" runat="server" Text="English Listening APP" style="z-index: 1; left: 290px; top: 5px; position: absolute" Font-Bold="true" Font-Size="XX-Large" Font-Underline="true" ></asp:Label>
        </div>
    
        <asp:Label ID="lblTime" runat="server" style="z-index: 1; left: 15px; top: 50px; position: absolute" ></asp:Label>
        
        <!-- Username box and labels -->
        <asp:TextBox ID="user" runat="server" style="z-index: 1; left: 400px; top: 300px; position: absolute" ></asp:TextBox>
        <asp:Label ID="lbluser" runat="server" Text="Username:" style="z-index: 1; left: 330px; top: 300px; position: absolute"></asp:Label>
        
        <!-- Password box and labels -->        
        <asp:TextBox ID="pass" runat="server" style="z-index: 1; left: 400px; top: 330px; position: absolute" ></asp:TextBox>
        <asp:Label ID="lblpass" runat="server" Text="Password:" style="z-index: 1; left: 330px; top: 330px; position: absolute"></asp:Label>
    
        <!-- Submit box and error message -->
        <asp:Button ID="Submit" runat="server" Text="Submit" style="z-index: 1; left: 430px; top: 370px; position: absolute" OnClick="Submit_Click" />
        <asp:Label ID="errormsg" runat="server" style="z-index: 1; left: 380px; top: 430px; position: absolute"></asp:Label>
        <asp:Label ID="incorrectmsg" runat="server" style="z-index: 1; left: 340px; top: 460px; position: absolute"></asp:Label>



        <asp:Label ID="Label2" runat="server" style="z-index: 1; left: 500px; top: 500px; position: absolute"></asp:Label>
        <asp:Label ID="Label3" runat="server" style="z-index: 1; left: 550px; top: 550px; position: absolute"></asp:Label>

    </form>
</body>
</html>
