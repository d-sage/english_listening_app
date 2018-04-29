<!-- Credit: https://www.codeproject.com/Questions/143724/how-to-add-Hyperlink-in-gridview-in-ASP-NET -->

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PDFViewer.aspx.cs" Inherits="PDFViewer" %>

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
        .scrolling-table-container 
        {
            display: inline-block;
            height: 500px;
            width: 100%;
            overflow-y: scroll;
            overflow-x: scroll;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnlogin" runat="server" Text="Login" style="z-index: 1; left: 15px; top: 100px; position: absolute; height: 26px;" OnClick="btnpdf_Click" />
            <asp:Label ID="SiteName" runat="server" Text="English Listening APP" style="z-index: 1; left: 330px; top: 0px; position: absolute" Font-Bold="true" Font-Size="72px" Font-Underline="true" ></asp:Label>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    
    
        <div class="scrolling-table-container">
            <!-- Table for Lesson Delete / Edit -->
            <asp:GridView ID="gridLesson" runat="server" AutoGenerateColumns="False" AutoPostBack="True">
                <Columns>
                    <asp:BoundField HeaderText="Country" DataField="cid"/>
                    <asp:BoundField HeaderText="Grade" DataField="gid"/>
                    <asp:BoundField HeaderText="Topic" DataField="tid"/>
                    <asp:BoundField HeaderText="Title" DataField="lid"/>
                    <asp:TemplateField HeaderText="File Link">
			            <ItemTemplate>
				            <asp:HyperLink ID="fileLink" runat="server" NavigateUrl='<%# Bind("path") %>' Text='<%# Bind("filename") %>'></asp:HyperLink>
			            </ItemTemplate>
		            </asp:TemplateField>
	            </Columns>
            </asp:GridView>
        </div>
    
    <asp:Label ID="lblerror" runat="server" style="z-index: 1; left: 500px; top: 500px; position: absolute"></asp:Label>
    </form>
</body>
</html>
