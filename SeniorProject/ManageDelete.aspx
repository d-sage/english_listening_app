<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageDelete.aspx.cs" Inherits="ManageDelete" %>

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
            <asp:Label ID="Label2" runat="server" Text="What Would You Like To Delete" style="z-index: 1; left: 350px; top: 50px; position: absolute"></asp:Label>
            <asp:Button ID="btnaddlink" Width="60" runat="server" Text="Add" style="z-index: 1; left: 580px; top: 50px; position: absolute" OnClick="Btnaddlink_Click" />

        </div>


        <!-- Table for Country Delete -->
        <asp:GridView ID="gridCountry" runat="server" style="z-index: 1; left: 84px; top: 134px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Country_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteCountry" runat="server" CommandName="delete country" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>

        <!-- Table for Topic Delete -->
        <asp:GridView ID="gridTopic" runat="server" style="z-index: 1; left: 225px; top: 134px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Topic_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Topic" DataField="tid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteTopic" runat="server" CommandName="delete topic" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>





        <!-- Error Labels -->
        <asp:Label ID="errormsgDB" runat="server" style="z-index: 1; left: 386px; top: 448px; position: absolute"></asp:Label>


         <!-- Display Countries -->
        <asp:BulletedList ID="blcountry" runat="server" style="z-index: 1; left: 85px; top: 492px; position: absolute" ></asp:BulletedList>

        <!-- Display Grades -->
        <asp:BulletedList ID="blgrades" runat="server" style="z-index: 1; left: 225px; top: 487px; position: absolute" ></asp:BulletedList>

        <!-- Display Topics -->
        <asp:BulletedList ID="bltopics" runat="server" style="z-index: 1; left: 370px; top: 487px; position: absolute" ></asp:BulletedList>

        <!-- Display Lessons -->
        <asp:BulletedList ID="bllessons" runat="server" style="z-index: 1; left: 522px; top: 483px; position: absolute" ></asp:BulletedList>

        

    </form>
</body>
</html>
