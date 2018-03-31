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
        <asp:GridView ID="gridCountry" runat="server" style="z-index: 1; left: 75px; top: 134px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Country_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteCountry" runat="server" CommandName="d" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit">
			        <ItemTemplate>
				        <asp:Button ID="editCountry" runat="server" CommandName="e" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>

        <!-- Table for Topic Delete -->
        <asp:GridView ID="gridTopic" runat="server" style="z-index: 1; left: 275px; top: 134px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Topic_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Topic" DataField="tid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteTopic" runat="server" CommandName="d" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit">
			        <ItemTemplate>
				        <asp:Button ID="editTopic" runat="server" CommandName="e" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>

        <!-- Table for Country_Grade Delete -->
        <asp:GridView ID="gridCountryGrade" runat="server" style="z-index: 1; left: 425px; top: 134px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="CountryGrade_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid"/>
                <asp:BoundField HeaderText="Grade" DataField="gid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteCountryGrade" runat="server" CommandName="d" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>

        <!-- Table for Country_Grade_Topic Delete -->
        <asp:GridView ID="gridCountryGradeTopic" runat="server" style="z-index: 1; left: 75px; top: 275px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="CountryGradeTopic_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid"/>
                <asp:BoundField HeaderText="Grade" DataField="gid"/>
                <asp:BoundField HeaderText="Topic" DataField="tid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteCountryGradeTopic" runat="server" CommandName="d" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>

        <!-- Table for Country_Grade Delete -->
        <asp:GridView ID="gridLesson" runat="server" style="z-index: 1; left: 625px; top: 134px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Lesson_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid"/>
                <asp:BoundField HeaderText="Grade" DataField="gid"/>
                <asp:BoundField HeaderText="Topic" DataField="tid"/>
                <asp:BoundField HeaderText="Title" DataField="lid"/>
                <asp:BoundField HeaderText="Filename" DataField="filename"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteLesson" runat="server" CommandName="d" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
			        </ItemTemplate>
		        </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit">
			        <ItemTemplate>
				        <asp:Button ID="editLesson" runat="server" CommandName="e" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>'/>
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
