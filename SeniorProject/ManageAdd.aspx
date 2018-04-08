<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageAdd.aspx.cs" Inherits="Manage" %>

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
            <asp:Label ID="SiteName" runat="server" Text="English Listening APP" style="z-index: 1; left: 330px; top: 0px; position: absolute" Font-Bold="true" Font-Size="72px" Font-Underline="true" ></asp:Label>
            <asp:Button ID="btndeletelink" runat="server" Text="Delete" style="z-index: 1; left: 740px; top: 85px; position: absolute" OnClick="Btndeletelink_Click" />
        </div>

        <!-- Info for opening web page -->
        <asp:Label ID="lblinfo" runat="server" Text="How would you like to change the database:" style="z-index: 1; left: 450px; top: 85px; position: absolute"></asp:Label>
        <asp:Label ID="lbladd" runat="server" Text="Add to Database:" style="z-index: 1; left: 175px; top: 100px; position: absolute"></asp:Label>


        <!-- Add to database controls -->
        <!-- Boxes and Labels for country add -->
        <asp:Label ID="lblCountryAdd" runat="server" Text="Country:" style="z-index: 1; left: 85px; top: 130px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtcountryAdd" runat="server" MaxLength="30" style="z-index: 1; left: 150px; top: 130px; position: absolute" ></asp:TextBox>
    
        <!-- Boxes and Labels for topic add -->
        <asp:Label ID="lbltopicAdd" runat="server" Text="Topic:" style="z-index: 1; left: 100px; top: 180px; position: absolute"></asp:Label>
        <asp:TextBox ID="txttopicAdd" runat="server" MaxLength="30" style="z-index: 1; left: 150px; top: 180px; position: absolute" ></asp:TextBox>

        <!-- Drop Boxes for Country Grade add -->
        <asp:Label ID="lblCountryGradeAdd" runat="server" Text="Country Grade:" style="z-index: 1; left: 44px; top: 240px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlCGcountry" runat="server" style="z-index: 1; left: 150px; top: 240px; position: absolute; width: 170px;" AutoPostBack="True" OnSelectedIndexChanged="CountryGrade_country_IndexChange"></asp:DropDownList>
        <asp:DropDownList ID="dlCGgrade" runat="server" style="z-index: 1; left: 350px; top: 240px; position: absolute; width: 170px;"></asp:DropDownList>

        <!-- Drop Boxes for Country Grade Topic add -->
        <asp:Label ID="lblCountryGradeTopicAdd" runat="server" Text="Country Grade Topic:" style="z-index: 1; left: 7px; top: 300px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlCGTcountrygrade" runat="server" style="z-index: 1; left: 150px; top: 300px; position: absolute; width: 210px;" AutoPostBack="True" OnSelectedIndexChanged="CountryGradeTopic_countrygrade_IndexChange"></asp:DropDownList>
        <asp:DropDownList ID="dlCGTtopic" runat="server" style="z-index: 1; left: 400px; top: 300px; position: absolute; width: 170px;"></asp:DropDownList>
        
        <!-- Boxes and Labels for lessons add -->
        <asp:Label ID="lbllessonAdd" runat="server" Text="Lesson:" style="z-index: 1; left: 95px; top: 350px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlLesson" runat="server" style="z-index: 1; left: 150px; top: 350px; position: absolute; width: 250px;"></asp:DropDownList>
        <asp:TextBox ID="txtLessonName" runat="server" MaxLength="30" style="z-index: 1; left: 442px; top: 350px; position: absolute; width: 190px;" ></asp:TextBox>
        <asp:FileUpload ID="fileMP3" runat="server" accept="audio/mpeg" ErrorMessage="Only mp3 files is allowed!" ValidationExpression ="^(.+(\.mp3|\.MP3))$" style="z-index: 1; left: 850px; top: 350px; position: absolute; right: 895px;"/>
        
        <!-- Submit buttons -->
        <asp:Button ID="addCountry" runat="server" Text="Add Country" style="z-index: 1; left: 350px; top: 130px; position: absolute" OnClick="AddCountry_Click" />
        <asp:Button ID="addTopic" runat="server" Text="Add Topic" style="z-index: 1; left: 350px; top: 180px; position: absolute; width: 109px;" OnClick="AddTopic_Click" />
        <asp:Button ID="addCountryGrade" runat="server" Text="Add Country Grade" style="z-index: 1; left: 550px; top: 240px; position: absolute" OnClick="AddCountryGrade_Click" />
        <asp:Button ID="addCountryGradeTopic" runat="server" Text="Add Country Grade Topic" style="z-index: 1; left: 600px; top: 300px; position: absolute;" OnClick="AddCountryGradeTopic_Click" />
        <asp:Button ID="addLesson" runat="server" Text="Add Lesson" style="z-index: 1; left: 1030px; top: 350px; position: absolute;" OnClick="AddLesson_Click" />
 


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









        <!-- TextBox for the text -->
        <asp:Label ID="lblLessonText" runat="server" Text="Enter text here:" style="z-index: 1; left: 700px; top: 350px; position: absolute;"></asp:Label>
        <asp:TextBox ID="tbtext" runat="server" TextMode="MultiLine" MaxLength="500" style="z-index: 1; left: 644px; top: 372px; position: absolute; height: 124px; width: 193px;"></asp:TextBox>
       

        <!-- Log TextBox -->
        <asp:TextBox ID="tblog" runat="server" TextMode="MultiLine" style="z-index: 1; left: 1119px; top: 0px; position: absolute; height: 700px; width: 250px;"></asp:TextBox>



       


        



       


    </form>




</body>



</html>
