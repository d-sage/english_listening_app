<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageAdd.aspx.cs" Inherits="Manage" %>

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
            <asp:Label ID="Label1" runat="server" Text="English Listening APP" style="z-index: 1; left: 276px; top: 8px; position: absolute" Font-Bold="true" Font-Size="XX-Large" Font-Underline="true" ></asp:Label>
            <asp:Button ID="btndeletelink" runat="server" Text="Delete" style="z-index: 1; left: 600px; top: 50px; position: absolute" OnClick="Btndeletelink_Click" />
        </div>

        <!-- Info for opening web page -->
        <asp:Label ID="lblinfo" runat="server" Text="How would you like to change the database:" style="z-index: 1; left: 300px; top: 50px; position: absolute"></asp:Label>
        <asp:Label ID="lbladd" runat="server" Text="Add to Database:" style="z-index: 1; left: 196px; top: 81px; position: absolute"></asp:Label>


        <!-- Add to database controls -->
        <!-- Boxes and Labels for country add -->
        <asp:Label ID="lblCountryAdd" runat="server" Text="Country:" style="z-index: 1; left: 95px; top: 130px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtcountryAdd" runat="server" style="z-index: 1; left: 180px; top: 130px; position: absolute" ></asp:TextBox>
    
        <!-- Boxes and Labels for topic add -->
        <asp:Label ID="lbltopicAdd" runat="server" Text="Topic:" style="z-index: 1; left: 95px; top: 180px; position: absolute"></asp:Label>
        <asp:TextBox ID="txttopicAdd" runat="server" style="z-index: 1; left: 180px; top: 180px; position: absolute" ></asp:TextBox>

        <!-- Drop Boxes for Country Grade add -->
        <asp:Label ID="lblCountryGradeAdd" runat="server" Text="Country Grade:" style="z-index: 1; left: 44px; top: 240px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlCGcountry" runat="server" style="z-index: 1; left: 179px; top: 236px; position: absolute; width: 170px;" AutoPostBack="True" OnSelectedIndexChanged="CountryGrade_country_IndexChange"></asp:DropDownList>
        <asp:DropDownList ID="dlCGgrade" runat="server" style="z-index: 1; left: 377px; top: 236px; position: absolute; width: 170px;"></asp:DropDownList>

        <!-- Drop Boxes for Country Grade Topic add -->
        <asp:Label ID="lblCountryGradeTopicAdd" runat="server" Text="Country Grade Topic:" style="z-index: 1; left: 26px; top: 301px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlCGTcountrygrade" runat="server" style="z-index: 1; left: 217px; top: 302px; position: absolute; width: 210px;" AutoPostBack="True" OnSelectedIndexChanged="CountryGradeTopic_countrygrade_IndexChange"></asp:DropDownList>
        <asp:DropDownList ID="dlCGTtopic" runat="server" style="z-index: 1; left: 475px; top: 303px; position: absolute; width: 170px;"></asp:DropDownList>
        
        <!-- Boxes and Labels for lessons add -->
        <asp:Label ID="lbllessonAdd" runat="server" Text="Lesson:" style="z-index: 1; left: 58px; top: 354px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlLesson" runat="server" style="z-index: 1; left: 149px; top: 354px; position: absolute; width: 250px;"></asp:DropDownList>
        <asp:TextBox ID="txtLessonName" runat="server" style="z-index: 1; left: 442px; top: 357px; position: absolute; width: 190px;" ></asp:TextBox>
        <asp:FileUpload ID="fileMP3" runat="server" accept="audio/mpeg" ErrorMessage="Only mp3 files is allowed!" ValidationExpression ="^(.+(\.mp3|\.MP3))$" style="z-index: 1; left: 254px; top: 396px; position: absolute; right: 895px;"/>
        
        <!-- Submit buttons -->
        <asp:Button ID="addCountry" runat="server" Text="Add Country" style="z-index: 1; left: 385px; top: 130px; position: absolute" OnClick="AddCountry_Click" />
        <asp:Button ID="addTopic" runat="server" Text="Add Topic" style="z-index: 1; left: 385px; top: 180px; position: absolute" OnClick="AddTopic_Click" />
        <asp:Button ID="addCountryGrade" runat="server" Text="Add Country Grade" style="z-index: 1; left: 584px; top: 232px; position: absolute" OnClick="AddCountryGrade_Click" />
        <asp:Button ID="addCountryGradeTopic" runat="server" Text="Add Country Grade Topic" style="z-index: 1; left: 684px; top: 301px; position: absolute;" OnClick="AddCountryGradeTopic_Click" />
        <asp:Button ID="addLesson" runat="server" Text="Add Lesson" style="z-index: 1; left: 704px; top: 354px; position: absolute;" OnClick="AddLesson_Click" />
        

        <!--Hidden field for number-->
        <asp:hiddenfield ID="ValueHiddenField" value="test" ClientIDMode="Static" runat="server"/>



        

        


        

        



        

        


        

        <!-- Testing Ajax Button
		<input id="btnTestAjax" type="button" value="Push To Test Ajax (Press F12 to see the console log)"/>
        -->


        <!-- Error Labels -->
        <asp:Label ID="errormsgDB" runat="server" style="z-index: 1; left: 386px; top: 448px; position: absolute"></asp:Label>


         <!-- Display Countries -->
        <asp:Label ID="countrydisplay" runat="server" style="z-index: 1; left: 92px; top: 450px; position: absolute"></asp:Label>
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
