<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Manage.aspx.cs" Inherits="Manage" %>

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


    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
	<script src="./Manage.js"></script>
    <script type = "text/javascript">
        var GettingValue = document.getElementById<%= ValueHiddenField.ClientID %>.value;
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="English Listening APP" style="z-index: 1; left: 290px; top: 5px; position: absolute" Font-Bold="true" Font-Size="XX-Large" Font-Underline="true" ></asp:Label>
        </div>

        <!-- Info for opening web page -->
        <asp:Label ID="lblinfo" runat="server" Text="How would you like to change the database:" style="z-index: 1; left: 300px; top: 50px; position: absolute"></asp:Label>
        <asp:Label ID="lbladd" runat="server" Text="Add to Database:" style="z-index: 1; left: 165px; top: 75px; position: absolute"></asp:Label>
        <asp:Label ID="lbldelete" runat="server" Text="Delete from Database:" style="z-index: 1; left: 600px; top: 75px; position: absolute"></asp:Label>


        <!-- Add to database controls -->
        <!-- Boxes and Labels for country, add side -->
        <asp:Label ID="lblCountryAdd" runat="server" Text="Country:" style="z-index: 1; left: 95px; top: 130px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtcountryAdd" runat="server" style="z-index: 1; left: 155px; top: 130px; position: absolute" ></asp:TextBox>
    
        <!-- Boxes and Labels for topic, add side -->
        <asp:Label ID="lbltopicAdd" runat="server" Text="Topic:" style="z-index: 1; left: 95px; top: 170px; position: absolute"></asp:Label>
        <asp:TextBox ID="txttopicAdd" runat="server" style="z-index: 1; left: 155px; top: 170px; position: absolute" ></asp:TextBox>

        <!-- Boxes and Labels for lessons, add side -->
        <asp:Label ID="lbllessonAdd" runat="server" Text="Lesson:" style="z-index: 1; left: 95px; top: 210px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtlessonAdd" runat="server" style="z-index: 1; left: 155px; top: 210px; position: absolute" ></asp:TextBox>


        <!-- Remove from database controls -->
        <!-- Boxes and Labels for Country, remove side -->
        <asp:Label ID="lblcountryRem" runat="server" Text="Country:" style="z-index: 1; left: 530px; top: 130px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtcountryrem" runat="server" style="z-index: 1; left: 590px; top: 130px; position: absolute" ></asp:TextBox>

        <!-- Boxes and Labels for Grade, remove side -->
        <asp:Label ID="lblgradeRem" runat="server" Text="Grade:" style="z-index: 1; left: 530px; top: 170px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtgradeRem" runat="server" style="z-index: 1; left: 590px; top: 170px; position: absolute" ></asp:TextBox>

        <!-- Boxes and Labels for Lesson, remove side -->
        <asp:Label ID="lbllessonRem" runat="server" Text="Lesson:" style="z-index: 1; left: 530px; top: 210px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtlessonRem" runat="server" style="z-index: 1; left: 590px; top: 210px; position: absolute" ></asp:TextBox>


        <!-- Submit buttons -->
        <asp:Button ID="addCountry" runat="server" Text="Add Country" style="z-index: 1; left: 350px; top: 300px; position: absolute" OnClick="AddCountry_Click" />
        <asp:Button ID="addTopic" runat="server" Text="Add Topic" style="z-index: 1; left: 450px; top: 300px; position: absolute" OnClick="AddTopic_Click" />
        

        <!--Hidden field for number-->
        <asp:hiddenfield ID="ValueHiddenField" value="test" ClientIDMode="Static" runat="server"/>

        <!-- Testing Ajax Button
		<input id="btnTestAjax" type="button" value="Push To Test Ajax (Press F12 to see the console log)"/>
        -->


        <!-- Error Labels -->
        <asp:Label ID="errormsgDB" runat="server" style="z-index: 1; left: 380px; top: 430px; position: absolute"></asp:Label>


         <!-- Display Countries -->
        <asp:Label ID="countrydisplay" runat="server" style="z-index: 1; left: 95px; top: 400px; position: absolute"></asp:Label>
        <asp:BulletedList ID="blcountry" runat="server" style="z-index: 1; left: 95px; top: 450px; position: absolute" ></asp:BulletedList>

        <!-- Display Grades -->
        <asp:BulletedList ID="blgrades" runat="server" style="z-index: 1; left: 200px; top: 450px; position: absolute" ></asp:BulletedList>

        <!-- Display Topics -->
        <asp:BulletedList ID="bltopics" runat="server" style="z-index: 1; left: 300px; top: 450px; position: absolute" ></asp:BulletedList>

        <!-- Display Lessons -->
        <asp:BulletedList ID="bllessons" runat="server" style="z-index: 1; left: 400px; top: 450px; position: absolute" ></asp:BulletedList>



    </form>




</body>



</html>
