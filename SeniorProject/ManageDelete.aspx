
<!-- Credit: https://asna.com/us/tech/kb/doc/scrolling-web-grid-->

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
        .scrolling-table-container {
            height: 200px;
            width: 80%;
            overflow-y: scroll;
            overflow-x: scroll;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="SiteName" runat="server" Text="English Listening APP" style="z-index: 1; left: 330px; top: 0px; position: absolute" Font-Bold="true" Font-Size="72px" Font-Underline="true" ></asp:Label>
                    <asp:Label ID="lblinfo" runat="server" Text="How would you like to change the database:" style="z-index: 1; left: 450px; top: 85px; position: absolute"></asp:Label>
            <asp:Button ID="btndeletelink" runat="server" Text="Add" style="z-index: 1; left: 740px; top: 85px; position: absolute; width: 65px;" OnClick="Btnaddlink_Click" />
        </div>

        <div class="scrolling-table-container">
        <!-- Table for Country Delete / Edit -->
        <asp:GridView ID="gridCountry" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Country_RowCommand" 
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
        </div>

        <div class="scrolling-table-container">
        <!-- Table for Topic Delete / Edit -->

        <asp:GridView ID="gridTopic" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Topic_RowCommand"
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
        </div>

        <div class="scrolling-table-container">
        <!-- Table for Country_Grade Delete -->
        
        <asp:GridView ID="gridCountryGrade" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="CountryGrade_RowCommand"
             OnRowDeleting="CountryGrade_OnRowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid"/>
                <asp:BoundField HeaderText="Grade" DataField="gid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteCountryGrade" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this country grade?');"/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>
        </div>

        <div class="scrolling-table-container">
        <!-- Table for Country_Grade_Topic Delete -->
         
        <asp:GridView ID="gridCountryGradeTopic" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="CountryGradeTopic_RowCommand"
            OnRowDeleting="CountryGradeTopic_OnRowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid"/>
                <asp:BoundField HeaderText="Grade" DataField="gid"/>
                <asp:BoundField HeaderText="Topic" DataField="tid"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteCountryGradeTopic" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this country grade?');"/>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>
        </div>

        <div class="scrolling-table-container">
        <!-- Table for Lesson Delete / Edit -->
        
        <asp:GridView ID="gridLesson" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowDataBound="Lesson_DataBound" OnRowCommand="Lesson_RowCommand"
            OnRowEditing="Lesson_OnRowEditing" OnRowCancelingEdit="Lesson_OnRowCancelingEdit" OnRowUpdating="Lesson_OnRowUpdating" OnRowDeleting="Lesson_OnRowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid" ReadOnly="true"/>
                <asp:BoundField HeaderText="Grade" DataField="gid" ReadOnly="true"/>
                <asp:BoundField HeaderText="Topic" DataField="tid" ReadOnly="true"/>
                <asp:BoundField HeaderText="Title" DataField="lid"/>
                <asp:BoundField HeaderText="Text" DataField="text"/>
                <asp:BoundField HeaderText="Filename" DataField="filename" ReadOnly="true"/>
		        <asp:TemplateField HeaderText="Delete">
			        <ItemTemplate>
				        <asp:Button ID="deleteLesson" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this country grade?');"/>
			        </ItemTemplate>
		        </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
	        </Columns>
        </asp:GridView>
        </div>
        
        <div>
        <!-- Error Labels -->
        <asp:Label ID="errormsgDB" runat="server" style="z-index: 1; left: 500px; top: 1000px; position: absolute"></asp:Label>


         <!-- Display Countries -->
        <asp:BulletedList ID="blcountry" runat="server"></asp:BulletedList>

        <!-- Display Grades -->
        <asp:BulletedList ID="blgrades" runat="server"></asp:BulletedList>

        <!-- Display Topics -->
        <asp:BulletedList ID="bltopics" runat="server"></asp:BulletedList>

        <!-- Display Lessons -->
        <asp:BulletedList ID="bllessons" runat="server"></asp:BulletedList>
        </div>
        

    </form>
</body>
</html>
