
<!-- Credit: https://asna.com/us/tech/kb/doc/scrolling-web-grid-->

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageDelete.aspx.cs" Inherits="ManageDelete" MaintainScrollPositionOnPostback="true" %>

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
            display: inline-block;
            height: 340px;
            overflow-y: scroll;
            overflow-x: scroll;
        }
        .inlineBlockDiv{
            display: inline-block;
            height: 290px;
            overflow-y: scroll;
            overflow-x: scroll;
        }
        .inlineDiv{
            display: inline;
        }
        .blockDiv{
            display: block;
        }
        div{
            padding-bottom: 5px;
        }
        .heading{
            width: 80%;
            text-align:center
        }
    </style>

</head>

<body>
    <form id="form1" runat="server">
        <div class="heading">
            <asp:Label ID="SiteName" runat="server" Text="English Listening APP" Font-Bold="true" Font-Size="72px" Font-Underline="true" ></asp:Label>
            <br />
            <asp:Label ID="lblinfo" runat="server" Text="How would you like to change the database:" ></asp:Label>
            <asp:Button ID="btndeletelink" runat="server" Text="Go To Add Page" OnClick="Btnaddlink_Click" />
            <br />
        </div>

        <div class="inlineBlockDiv">
            <!-- Table for Country Delete / Edit -->
            <asp:GridView ID="gridCountry" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="Country_RowCommand" 
                OnRowEditing="Country_OnRowEditing" OnRowCancelingEdit="Country_OnRowCancelingEdit" OnRowUpdating="Country_OnRowUpdating" OnRowDeleting="Country_OnRowDeleting" >
                <Columns>
                    <asp:BoundField HeaderText="Country" DataField="cid" ReadOnly="false"/>
                    <asp:TemplateField HeaderText="Delete">
			            <ItemTemplate>
				            <asp:Button ID="deleteCountry" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this country?');"/>
			            </ItemTemplate>
		            </asp:TemplateField>
                    <asp:CommandField HeaderText="Edit" ShowEditButton="true" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
	            </Columns>
            </asp:GridView>
        </div>

        <div class="inlineBlockDiv">
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
                    <asp:CommandField HeaderText="Edit" ShowEditButton="true" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
	            </Columns>
            </asp:GridView>
        </div>

        <div class="inlineBlockDiv">
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

        <div class="inlineBlockDiv">
            <!-- Table for Country_Grade_Topic Delete -->
            <asp:GridView ID="gridCountryGradeTopic" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowCommand="CountryGradeTopic_RowCommand"
                OnRowDeleting="CountryGradeTopic_OnRowDeleting">
                <Columns>
                    <asp:BoundField HeaderText="Country" DataField="cid"/>
                    <asp:BoundField HeaderText="Grade" DataField="gid"/>
                    <asp:BoundField HeaderText="Topic" DataField="tid"/>
		            <asp:TemplateField HeaderText="Delete">
			            <ItemTemplate>
				            <asp:Button ID="deleteCountryGradeTopic" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this country grade topic? This will remove all lessons associated with it as well.');"/>
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
                    <asp:CommandField HeaderText="Edit" ShowEditButton="true" ButtonType="Button" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
	            </Columns>
            </asp:GridView>
        </div>
        
        
        
        
        
        
        
        <div>
            <!-- Error Labels -->
            <asp:Label ID="errormsgDB" runat="server" style="z-index: 1; left: 500px; top: 1000px; position: absolute"></asp:Label>
            <asp:TextBox ID="tblog" runat="server" TextMode="MultiLine" style="z-index: 1; left: 1079px; top: 20px; position: absolute; height: 750px; width: 264px;"></asp:TextBox>
            <asp:Label ID="lblLogBox" runat="server" Text="Log Box" style="z-index: 1; left: 1026px; top: 0px; position: absolute;"></asp:Label>
            <asp:Button ID="btnClearLog" runat="server" Text="Clear Log" style="z-index: 1; left: 1000px; top: 0px; position: absolute;" OnClick="ClearLog_Click"/>
        </div>
        

    </form>
</body>
</html>
