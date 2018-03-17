using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Runtime.Serialization.Json;

public partial class Manage : System.Web.UI.Page
{
    private int num;
    private String stringhelper;
    private int inthelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();


    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();
        //makes sure they aren't going around the login
        if(run)
        {

            ValueHiddenField.Value = num.ToString();
            
            if (!Page.IsPostBack)
            {
                UpdateAllData();
            }
            
            

            //*all code goes in here*


        }//end if
        else
            Response.Redirect("Login.aspx");

    }//end main method

    #endregion Page_Load

    #region GetConnectionString

    private string GetConnectionString()
    {

        /*
         string text = "";
            string server = "162.241.244.134";
            string database = "jordape8_EnglishApp";
            string uid = "jordape8_Default";
            string password = "Default1!";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */
        
        string server = "localhost";
        string database = "daricsag_ela";
        string uid = "daricsag_ela";
        string password = "english";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        return connectionString;
    }

    #endregion GetConnectionString

    #region Get SQL Connection

    private MySqlConnection GetSqlConnection()
    {

        string text = "";
        string server = "162.241.244.134";
        string database = "jordape8_EnglishApp";
        string uid = "jordape8_Default";
        string password = "Default1!";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        //local testing >>>
        connectionString = GetConnectionString();
        //local testing <<<

        return new MySqlConnection(connectionString);
    }

    #endregion Get SQL Connection

    #region UpdateData

    #region Update All Data

    private void UpdateAllData()
    {

        BlanksOnAllDropLists();

        DisableBoxes();

        String text = "Good";

        MySqlConnection connection = GetSqlConnection();

        try
        {

            UpdateCountries(connection);
            
            UpdateGrades(connection);
            
            UpdateTopics(connection);

            UpdateCountryGrade(connection);
            
            UpdateLessons(connection);
            
        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch
        
        errormsgDB.Text = text;
        
    }

    #endregion Update All Data

    #region Update Countries

    private void UpdateCountries(MySqlConnection connection)
    {

        connection.Open();

        BlanksOnDropList(dlCGcountry);

        //display the countries
        String sql = "SELECT * FROM countries";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            //add to bulletlist display
            stringhelper = (String)rdr[0];
            listcountry.Add(stringhelper);

            //add to country droplists
            dlCGcountry.Items.Add(new ListItem((String)rdr[0], (String)rdr[0]));

        }//end while
        rdr.Close();
        connection.Close();

        blcountry.DataSource = listcountry;
        blcountry.DataBind();
    }

    #endregion Update Countries

    #region Update Grades

    private void UpdateGrades(MySqlConnection connection)
    {
        connection.Open();
        //TODO
        //BlanksOnDropList(dlCGcountry);

        String sql = "SELECT * FROM grades";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            stringhelper = rdr[0].ToString();
            listgrade.Add(stringhelper);
        }//end while
        rdr.Close();
        connection.Close();

        blgrades.DataSource = listgrade;
        blgrades.DataBind();
    }

    #endregion Update Grades

    #region Update Topics

    private void UpdateTopics(MySqlConnection connection)
    {
        connection.Open();
        //TODO
        //BlanksOnDropList(dlCGcountry);

        String sql = "SELECT * FROM topics";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            stringhelper = (String)rdr[0];
            listtopic.Add(stringhelper);
        }//end while
        rdr.Close();
        connection.Close();

        bltopics.DataSource = listtopic;
        bltopics.DataBind();
    }

    #endregion Update Topics

    #region Update Country_Grade

    private void UpdateCountryGrade(MySqlConnection connection)
    {

        connection.Open();

        BlanksOnDropList(dlCGTcountrygrade);

        //display the countries
        String sql = "SELECT * FROM country_grade_relationship";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {

            //add to countrygrade droplist
            dlCGTcountrygrade.Items.Add(new ListItem((String)rdr[0] + " " + rdr[1].ToString(), (String)rdr[0] + " " + rdr[1].ToString()));

        }//end while
        rdr.Close();
        connection.Close();
        
    }

    #endregion Update Country_Grade

    #region Update Lessons

    private void UpdateLessons(MySqlConnection connection)
    {
        connection.Open();

        String sql = "SELECT * FROM lessons";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            stringhelper = rdr[0] + " | " +
                            rdr[1].ToString() + " | " +
                            rdr[2] + " | " +
                            rdr[3] + " | " +
                            rdr[4] + " | " +
                            rdr[5] + " | " +
                            rdr[6];
            listlesson.Add(stringhelper);
        }//end while
        rdr.Close();
        connection.Close();

        bllessons.DataSource = listlesson;
        bllessons.DataBind();
    }

    #endregion Update Lessons

    #endregion UpdateData

    #region Disable Boxes

    private void DisableBoxes()
    {
        
        //for country_grade add
        dlCGgrade.Enabled = false;

        //for country_grade_topic add
        dlCGTtopic.Enabled = false;

    }

    private void DisableBox(DropDownList ddl)
    {
        ddl.Enabled = false;
    }

    #endregion Disable Boxes

    #region BlanksOnDropLists Methods

    private void BlanksOnDropList(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(String.Empty, String.Empty));
        ddl.SelectedIndex = 0;
    }

    private void BlanksOnAllDropLists()
    {
        //initiate dropdownlists to have a blank

        //for country_grade
        dlCGcountry.Items.Clear();
        dlCGcountry.Items.Add(new ListItem(String.Empty, String.Empty));
        dlCGcountry.SelectedIndex = 0;

        dlCGgrade.Items.Clear();
        dlCGgrade.Items.Add(new ListItem(String.Empty, String.Empty));
        dlCGgrade.SelectedIndex = 0;

        //for country_grade_topic
        dlCGTcountrygrade.Items.Clear();
        dlCGTcountrygrade.Items.Add(new ListItem(String.Empty, String.Empty));
        dlCGTcountrygrade.SelectedIndex = 0;

        dlCGTtopic.Items.Clear();
        dlCGTtopic.Items.Add(new ListItem(String.Empty, String.Empty));
        dlCGTtopic.SelectedIndex = 0;


    }

    private void BlanksOnAllDropLists_exceptCountries()
    {
        /*
        dlCGgrade.Items.Clear();
        dlCGgrade.Items.Add(new ListItem(String.Empty, String.Empty));
        dlCGgrade.SelectedIndex = 0;
        */
    }

    #endregion BlanksOnDropLists Methods

    #region AddCountry_Click

    protected void AddCountry_Click(object sender, EventArgs e)
    {

        if (txtcountryAdd.Text.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country: field not filled');", true);
            return;
        }

        string country = txtcountryAdd.Text;

        string text = "Good";
        
        string connectionString = GetConnectionString();
        
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "INSERT INTO countries (cid) VALUES (@cid)";
            
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);

            cmd.ExecuteNonQuery();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        txtcountryAdd.Text = "";

        UpdateCountries(GetSqlConnection());
        
    }//end method

    #endregion AddCountry_Click

    #region AddTopic_Click

    protected void AddTopic_Click(object sender, EventArgs e)
    {

        if (txttopicAdd.Text.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Topic: field not filled');", true);
            return;
        }

        string topic = txttopicAdd.Text;

        string text = "Good";

        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "INSERT INTO topics (tid) VALUES (@tid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@tid", topic);

            cmd.ExecuteNonQuery();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        txttopicAdd.Text = "";

        UpdateTopics(GetSqlConnection());

    }//end method

    #endregion AddTopic_Click

    #region Add to CountryGrade

    #region CountryGrade_country_TextChange

    protected void CountryGrade_country_IndexChange(object sender, EventArgs e)
    {
        
        BlanksOnDropList(dlCGgrade);
        DisableBox(dlCGgrade);

        if (dlCGcountry.Text.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country Grade: country field not filled');", true);
            return;
        }
        
        bool canContinue = true;

        string country = dlCGcountry.Text;

        string text = "Good";

        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            
            //this selects all the gid from the 'grades' tables that does not yet
            //have an association with the given country
            String sql = "SELECT gid " +
                         "FROM grades " +
                         "WHERE gid NOT IN " +
                            "(SELECT gid FROM country_grade_relationship WHERE cid = (@cid));";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);

            MySqlDataReader rdr = cmd.ExecuteReader();

            if(!rdr.HasRows)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country Grade: no content to add');", true);
                canContinue = false;
            }

            while (rdr.Read())
            {

                //add to dlCGgrade
                dlCGgrade.Items.Add(new ListItem(rdr[0].ToString()));

            }//end while
            rdr.Close();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        if(!canContinue)
        {
            UpdateAllData();
        }

        dlCGgrade.Enabled = canContinue;

    }

    #endregion CountryGrade_country_TextChange

    #region AddCountryGrade_Click

    protected void AddCountryGrade_Click(object sender, EventArgs e)
    {
        if (dlCGcountry.Text.Length == 0 || dlCGgrade.Text.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country Grade: not all fields filled');", true);
            return;
        }

        String country = dlCGcountry.Text;
        String grade = dlCGgrade.Text;

        string text = "Good";

        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "INSERT INTO country_grade_relationship (cid,gid) VALUES (@cid,@gid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);
            cmd.Parameters.AddWithValue("@gid", grade);

            cmd.ExecuteNonQuery();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        UpdateAllData();

    }

    #endregion AddCountryGrade_Click

    #endregion Add to CountryGrade

    #region Add to CountryGradeTopic

    #region CountryGradeTopic_countrygrade_TextChange

    protected void CountryGradeTopic_countrygrade_IndexChange(object sender, EventArgs e)
    {

        BlanksOnDropList(dlCGTtopic);
        DisableBox(dlCGTtopic);

        if (dlCGTcountrygrade.Text.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country Grade Topic: country_grade field not filled');", true);
            return;
        }

        bool canContinue = true;
        
        string tempCountryGrade = dlCGTcountrygrade.Text;
        string[] countrygradeSplit = tempCountryGrade.Split(' ');
        string country = countrygradeSplit[0];
        string grade = countrygradeSplit[1];
        
        string text = "Good";

        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            //this selects all the tid from the 'topics' table that does not yet
            //have an association with the given country_grade
            String sql = "SELECT tid " +
                         "FROM topics " +
                         "WHERE tid NOT IN " +
                            "(SELECT tid FROM country_grade_topic_relation WHERE cid = (@cid) AND gid = (@gid));";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);
            cmd.Parameters.AddWithValue("@gid", grade);

            MySqlDataReader rdr = cmd.ExecuteReader();

            if (!rdr.HasRows)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country Grade Topic: no content to add');", true);
                canContinue = false;
            }

            while (rdr.Read())
            {

                //add to dlCGgrade
                dlCGTtopic.Items.Add(new ListItem(rdr[0].ToString()));

            }//end while
            rdr.Close();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        if (!canContinue)
        {
            UpdateAllData();
        }
        
        dlCGTtopic.Enabled = canContinue;

    }

    #endregion CountryGradeTopic_countrygrade_TextChange

    #region AddCountryGradeTopic_Click

    protected void AddCountryGradeTopic_Click(object sender, EventArgs e)
    {
        if (dlCGTcountrygrade.Text.Length == 0 || dlCGTtopic.Text.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country Grade Topic: not all fields filled');", true);
            return;
        }

        string tempCountryGrade = dlCGTcountrygrade.Text;
        string[] countrygradeSplit = tempCountryGrade.Split(' ');
        string country = countrygradeSplit[0];
        string grade = countrygradeSplit[1];
        string topic = dlCGTtopic.Text;

        string text = "Good";

        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "INSERT INTO country_grade_topic_relation (cid,gid,tid) VALUES (@cid,@gid,@tid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);
            cmd.Parameters.AddWithValue("@gid", grade);
            cmd.Parameters.AddWithValue("@tid", topic);

            cmd.ExecuteNonQuery();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        UpdateAllData();

    }

    #endregion AddCountryGradeTopic_Click

    #endregion Add to CountryGradeTopic

    #region AddLesson_Click

    #endregion AddLesson_Click

    #region GetSession

    private bool GetSession()
    {
        /*if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            num = (int)Session["number"];
            if (matching)
                return true;
        }//end else*/
            return true;
    }//end method

    #endregion GetSession

    #region MySqlExceptionHandler

    private string MySqlExceptionHandler(int exceptionNum)
    {
        //When handling errors, you can your application's response based 
        //on the error number.
        //The two most common error numbers when connecting are as follows:
        //0: Cannot connect to server.
        //1045: Invalid user name and/or password.
        switch (exceptionNum)
        {
            case 0:
                return "Cannot connect to server.  Contact administrator";
            case 1045:
                return "Invalid username/password, please try again";
            default:
                return "number: " + exceptionNum;
        }//end switch
    }

    #endregion MySqlExceptionHandler


    #region Unused Ajax

    private bool SetAjaxSession()
    {

        bool isGood = true;

        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        int secondsSinceEpoch = (int)t.TotalSeconds;

        /*
        string text = "Good";
        string server = "162.241.244.134";
        string database = "jordape8_EnglishApp";
        string uid = "jordape8_Default";
        string password = "Default1!";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */
        string text = "Good";
        string server = "localhost";
        string database = "daricsag_ela";
        string uid = "daricsag_ela";
        string password = "english";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";


        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "DELETE FROM session";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            
            cmd.CommandText = "INSERT INTO session (id,time) VALUES(@id, @time)";
            cmd.Parameters.AddWithValue("@id", num);
            cmd.Parameters.AddWithValue("@time", secondsSinceEpoch);
            cmd.ExecuteNonQuery();

        }//end try
        catch (MySqlException ex)
        {
            //When handling errors, you can your application's response based 
            //on the error number.
            //The two most common error numbers when connecting are as follows:
            //0: Cannot connect to server.
            //1045: Invalid user name and/or password.
            switch (ex.Number)
            {
                case 0:
                    text = "Cannot connect to server.  Contact administrator";
                    break;

                case 1045:
                    text = "Invalid username/password, please try again";
                    break;
                default:
                    text = "number: " + ex.Number;
                    break;
            }//end switch
            text += " bad";
            isGood = false;
        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return isGood;
    }

    [WebMethod]
    public static string AjaxSendTest(object data)
    {

        /*
        Dictionary<string, object> d = (Dictionary<string, object>)data;

        List<string> keyList = new List<string>(d.Keys);
        
        object tt = d["Data"];

        object[] oa = (object[])tt;

        object one = oa[1];
        */

        return "here";

    }

    #endregion Unused Ajax


}//end class