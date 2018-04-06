<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageDelete.aspx.cs" Inherits="ManageDelete" %>

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
            <asp:Label ID="Label1" runat="server" Text="English Listening APP" style="z-index: 1; left: 290px; top: 5px; position: absolute" Font-Bold="true" Font-Size="XX-Large" Font-Underline="true" ></asp:Label>
            <asp:Label ID="Label2" runat="server" Text="What Would You Like To Delete" style="z-index: 1; left: 350px; top: 50px; position: absolute"></asp:Label>
            <asp:Button ID="btnaddlink" Width="60" runat="server" Text="Add" style="z-index: 1; left: 580px; top: 50px; position: absolute" OnClick="Btnaddlink_Click" />

        </div>


        <!-- Table for Country Delete -->
        <asp:GridView ID="gridCountry" runat="server" style="z-index: 1; left: 15px; top: 150px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Country_RowCommand"
            OnRowEditing="Country_OnRowEditing" OnRowCancelingEdit="Country_OnRowCancelingEdit" OnRowUpdating="Country_OnRowUpdating" OnRowDeleting="Country_OnRowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid" ReadOnly="false"/>
                <asp:TemplateField>
			        <ItemTemplate>
				        <asp:Button ID="deleteCountry" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this country?');"/>
			        </ItemTemplate>
		        </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
	        </Columns>
        </asp:GridView>

        <!-- Table for Topic Delete -->
        <asp:GridView ID="gridTopic" runat="server" style="z-index: 1; left: 500px; top: 134px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Topic_RowCommand"
            OnRowEditing="Topic_OnRowEditing" OnRowCancelingEdit="Topic_OnRowCancelingEdit" OnRowUpdating="Topic_OnRowUpdating" OnRowDeleting="Topic_OnRowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Topic" DataField="tid" ReadOnly="false"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteTopic" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this topic?');"/>
			        </ItemTemplate>
		        </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
	        </Columns>
        </asp:GridView>



        <!-- Table for Country_Grade Delete -->
        <asp:GridView ID="gridCountryGrade" runat="server" style="z-index: 1; left: 425px; top: 300px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="CountryGrade_RowCommand">
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
        <asp:GridView ID="gridCountryGradeTopic" runat="server" style="z-index: 1; left: 75px; top: 475px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="CountryGradeTopic_RowCommand">
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
        <asp:GridView ID="gridLesson" runat="server" style="z-index: 1; left: 625px; top: 675px; position: absolute" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Lesson_RowCommand">
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
