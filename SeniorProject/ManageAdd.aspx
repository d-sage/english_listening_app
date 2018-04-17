<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageAdd.aspx.cs" Inherits="Manage" MaintainScrollPositionOnPostback="true" %>

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
            height: 400px;
            width: 100%;
            overflow-y: scroll;
            overflow-x: scroll;
        }
    </style>




</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="SiteName" runat="server" Text="English Listening APP" style="z-index: 1; left: 330px; top: 0px; position: absolute" Font-Bold="true" Font-Size="72px" Font-Underline="true" ></asp:Label>
            <asp:Button ID="btndeletelink" runat="server" Text="Go To Delete/Edit Page" style="z-index: 1; left: 740px; top: 85px; position: absolute" OnClick="Btndeletelink_Click" />
        </div>

        <!-- Info for opening web page -->
        <asp:Label ID="lblinfo" runat="server" Text="How would you like to change the database:" style="z-index: 1; left: 450px; top: 85px; position: absolute"></asp:Label>
        <asp:Label ID="lbladd" runat="server" Text="Add to Database:" style="z-index: 1; left: 175px; top: 100px; position: absolute"></asp:Label>


        <!-- Add to database controls -->
        <!-- Boxes and Labels for country add -->
        <asp:Label ID="lblCountryAdd" runat="server" Text="Add a Country:" style="z-index: 1; left: 42px; top: 130px; position: absolute"></asp:Label>
        <asp:TextBox ID="txtcountryAdd" runat="server" MaxLength="30" style="z-index: 1; left: 150px; top: 130px; position: absolute" ></asp:TextBox>
    
        <!-- Boxes and Labels for topic add -->
        <asp:Label ID="lbltopicAdd" runat="server" Text="Add a Topic:" style="z-index: 1; left: 60px; top: 180px; position: absolute"></asp:Label>
        <asp:TextBox ID="txttopicAdd" runat="server" MaxLength="50" style="z-index: 1; left: 150px; top: 180px; position: absolute" ></asp:TextBox>

        <!-- Drop Boxes for Country Grade add -->
        <asp:Label ID="lblCountryGradeAdd" runat="server" Text="Add a Grade<br>to a Country:" style="z-index: 1; left: 57px; top: 230px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlCGcountry" runat="server" style="z-index: 1; left: 150px; top: 240px; position: absolute; width: 170px;" AutoPostBack="True" OnSelectedIndexChanged="CountryGrade_country_IndexChange"></asp:DropDownList>
        <asp:DropDownList ID="dlCGgrade" runat="server" style="z-index: 1; left: 350px; top: 240px; position: absolute; width: 170px;"></asp:DropDownList>

        <!-- Drop Boxes for Country Grade Topic add -->
        <asp:Label ID="lblCountryGradeTopicAdd" runat="server" Text="Add a Topic to a<br>Country's Grade:" style="z-index: 1; left: 31px; top: 290px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlCGTcountrygrade" runat="server" style="z-index: 1; left: 150px; top: 300px; position: absolute; width: 210px;" AutoPostBack="True" OnSelectedIndexChanged="CountryGradeTopic_countrygrade_IndexChange"></asp:DropDownList>
        <asp:DropDownList ID="dlCGTtopic" runat="server" style="z-index: 1; left: 400px; top: 300px; position: absolute; width: 170px;"></asp:DropDownList>
        
        <!-- Boxes and Labels for lessons add -->
        <asp:Label ID="lbllessonAdd" runat="server" Text="Add a Lesson:" style="z-index: 1; left: 50px; top: 350px; position: absolute"></asp:Label>
        <asp:DropDownList ID="dlLesson" runat="server" style="z-index: 1; left: 150px; top: 350px; position: absolute; width: 250px;"></asp:DropDownList>
        <asp:TextBox ID="txtLessonName" runat="server" MaxLength="100" style="z-index: 1; left: 410px; top: 350px; position: absolute; width: 190px;" ></asp:TextBox>
        <asp:FileUpload ID="fileMP3" runat="server" accept="audio/mpeg" ErrorMessage="Only mp3 files is allowed!" ValidationExpression ="^(.+(\.mp3|\.MP3))$" style="z-index: 1; left: 810px; top: 350px; position: absolute; right: 895px;"/>
        
        <!-- Submit buttons -->
        <asp:Button ID="addCountry" runat="server" Text="Add Country" style="z-index: 1; left: 350px; top: 130px; position: absolute" OnClick="AddCountry_Click" />
        <asp:Button ID="addTopic" runat="server" Text="Add Topic" style="z-index: 1; left: 350px; top: 180px; position: absolute; width: 109px;" OnClick="AddTopic_Click" />
        <asp:Button ID="addCountryGrade" runat="server" Text="Add Country Grade" style="z-index: 1; left: 550px; top: 240px; position: absolute" OnClick="AddCountryGrade_Click" />
        <asp:Button ID="addCountryGradeTopic" runat="server" Text="Add Country Grade Topic" style="z-index: 1; left: 600px; top: 300px; position: absolute;" OnClick="AddCountryGradeTopic_Click" />
        <asp:Button ID="addLesson" runat="server" Text="Add Lesson" style="z-index: 1; left: 1000px; top: 350px; position: absolute;" OnClick="AddLesson_Click" />
 


        <!-- Error Labels -->
        <asp:Label ID="errormsgDB" runat="server" style="z-index: 1; left: 386px; top: 448px; position: absolute"></asp:Label>


         <!-- Display Countries 
        <asp:BulletedList ID="blcountry" runat="server" BorderStyle="Solid" BorderWidth="1" style="z-index: 1; left: 100px; top: 650px; position: absolute" ></asp:BulletedList>

        <!-- Display Grades 
        <asp:BulletedList ID="blgrades" runat="server" BorderStyle="Solid" BorderWidth="1" style="z-index: 1; left: 230px; top: 650px; position: absolute" ></asp:BulletedList>

        <!-- Display Topics 
        <asp:BulletedList ID="bltopics" runat="server" BorderStyle="Solid" BorderWidth="1" style="z-index: 1; left: 350px; top: 650px; position: absolute" ></asp:BulletedList>
             
        <!-- Display Lessons 
        <asp:BulletedList ID="bllessons" runat="server" BorderStyle="Solid" BorderWidth="1" style="z-index: 1; left: 550px; top: 650px; position: absolute" ></asp:BulletedList>
        -->



        <div class="scrolling-table-container" style="z-index: 1; left: 0px; top: 580px; position: absolute">
        <!-- Table for Lesson Display -->
        
        <asp:GridView ID="gridLesson" runat="server" AutoGenerateColumns="False" AutoPostBack="True" OnRowDataBound="Lesson_DataBound">
            <Columns>
                <asp:BoundField HeaderText="Country" DataField="cid" ReadOnly="true"/>
                <asp:BoundField HeaderText="Grade" DataField="gid" ReadOnly="true"/>
                <asp:BoundField HeaderText="Topic" DataField="tid" ReadOnly="true"/>
                <asp:BoundField HeaderText="Title" DataField="lid"/>
                <asp:BoundField HeaderText="Text" DataField="text"/>
                <asp:BoundField HeaderText="Filename" DataField="filename" ReadOnly="true"/>
	        </Columns>
        </asp:GridView>
        </div>





        <!-- TextBox for the text -->
        <asp:Label ID="lblLessonText" runat="server" Text="Enter text here:" style="z-index: 1; left: 660px; top: 330px; position: absolute;"></asp:Label>
        <asp:TextBox ID="tbtext" runat="server" TextMode="MultiLine" MaxLength="2500" style="z-index: 1; left: 610px; top: 350px; position: absolute; height: 216px; width: 193px;"></asp:TextBox>
       

        <!-- Log TextBox/ title name -->
        <asp:TextBox ID="tblog" runat="server" TextMode="MultiLine" style="z-index: 1; left: 1103px; top: 20px; position: absolute; height: 563px; width: 240px;"></asp:TextBox>
        <asp:Label ID="lblLogBox" runat="server" Text="Log Box" style="z-index: 1; left: 1150px; top: 0px; position: absolute;"></asp:Label>
        <asp:Button ID="btnClearLog" runat="server" Text="Clear Log" style="z-index: 1; left: 1024px; top: 0px; position: absolute;" OnClick="ClearLog_Click"/>
        <asp:Label ID="lbltitle" runat="server" Text="Title:" style="z-index: 1; left: 490px; top: 330px; position: absolute; bottom: 125px;"></asp:Label>



       


        



       


        



       


        



       





       


        



       


        



       


       



       


    </form>




</body>



</html>
