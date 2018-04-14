using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Data;

public partial class Manage : System.Web.UI.Page
{
    private int num;
    private String stringhelper;
    private int inthelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();

    //Constants
    private const int DB_COUNTRY_LENGTH_MAX = 30;
    private const int DB_TOPIC_LENGTH_MAX = 50;
    private const int DB_LID_LENGTH_MAX = 100;
    private const int DB_TEXT_LENGTH_MAX = 2500;
    private const int FILE_NAME_LENGTH_MAX = 50;
    private const int FILE_SIZE_MAX = 15728640;

    private const string DROPLIST_COUNTRY_TEXT = "country...";
    private const string DROPLIST_TOPIC_TEXT = "topic...";
    private const string DROPLIST_GRADE_TEXT = "grade...";
    private const string DROPLIST_COUNTRY_GRADE_TEXT = "country grade...";
    private const string DROPLIST_COUNTRY_GRADE_TOPIC_TEXT = "country grade topic...";

    //ClientScript.RegisterStartupScript(this.GetType(), "Success", "alert('Lesson: Successfully Added');", true);

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();

        //makes sure they aren't going around the login
        if (run)
        {
            
            if (!Page.IsPostBack)
            {
                UpdateAllData();
            }
            
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
            string server = "localhost";  //162.241.244.134
            string database = "jordape8_EnglishApp";
            string uid = "jordape8_admin";
            string password = "Admin123";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */

        /*
        string server = "localhost";
        string database = "daricsag_ela";
        string uid = "daricsag_ela";
        string password = "english";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */

        
        string server = "mysql5018.site4now.net";
        string database = "db_a38d8d_lambe";
        string uid = "a38d8d_lambe";
        string password = "Lambejor000";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        

        return connectionString;
    }

    #endregion GetConnectionString

    #region Get SQL Connection

    private MySqlConnection GetSqlConnection()
    {
        string connectionString = GetConnectionString();
        MySqlConnection conn = null;

        try
        {
            conn = new MySqlConnection(connectionString);
        }
        catch(Exception e)
        {
            EmailError(e.ToString());
            throw new ArgumentException();
        }

        return conn;
    }

    #endregion Get SQL Connection

    #region UpdateData

    #region Update All Data

    private void UpdateAllData()
    {

        BlanksOnAllDropLists_WithText();

        DisableBoxes();

        String text = "";
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();
        }
        catch(ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }

        UpdateCountries(connection);

        UpdateGrades(connection);

        UpdateTopics(connection);

        UpdateCountryGrade(connection);

        UpdateCountryGradeTopic(connection);

        UpdateLessons(connection);
        
    }

    #endregion Update All Data

    #region Update Countries

    private void UpdateCountries(MySqlConnection connection)
    {

        BlanksOnDropList_WithText(dlCGcountry, DROPLIST_COUNTRY_TEXT);

        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT cid FROM countries ORDER BY cid ASC";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        //add to bulletlist display
                        stringhelper = (String)rdr[0];
                        listcountry.Add(stringhelper);

                        //add to country droplists
                        dlCGcountry.Items.Add(new ListItem((String)rdr[0], (String)rdr[0]));

                    }//end while
                }
            }
        }
        catch(MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch(Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        blcountry.DataSource = listcountry;
        blcountry.DataBind();
    }

    #endregion Update Countries

    #region Update Grades

    private void UpdateGrades(MySqlConnection connection)
    {

        try
        {
            connection.Open();

            String sql = "SELECT * FROM grades";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        stringhelper = rdr[0].ToString();
                        listgrade.Add(stringhelper);
                    }//end while
                }
            }
        }
        catch(MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch(Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        blgrades.DataSource = listgrade;
        blgrades.DataBind();
    }

    #endregion Update Grades

    #region Update Topics

    private void UpdateTopics(MySqlConnection connection)
    {

        try
        {
            connection.Open();

            String sql = "SELECT tid FROM topics ORDER BY tid ASC";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        stringhelper = (String)rdr[0];
                        listtopic.Add(stringhelper);
                    }//end while
                }
            }
        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        bltopics.DataSource = listtopic;
        bltopics.DataBind();
    }

    #endregion Update Topics

    #region Update Country_Grade

    private void UpdateCountryGrade(MySqlConnection connection)
    {

        BlanksOnDropList_WithText(dlCGTcountrygrade, DROPLIST_COUNTRY_GRADE_TEXT);

        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT cid,gid FROM country_grade_relationship ORDER BY cid,gid ASC";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {

                        //add to countrygrade droplist
                        dlCGTcountrygrade.Items.Add(new ListItem((String)rdr[0] + " ~~ " + rdr[1].ToString(), (String)rdr[0] + "|" + rdr[1].ToString()));

                    }//end while
                }
            }
        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

    }

    #endregion Update Country_Grade

    #region Update Country_Grade_Topic

    private void UpdateCountryGradeTopic(MySqlConnection connection)
    {

        BlanksOnDropList_WithText(dlLesson, DROPLIST_COUNTRY_GRADE_TOPIC_TEXT);

        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT cid,gid,tid FROM country_grade_topic_relation ORDER BY cid,gid,tid ASC";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {

                        //add to countrygrade droplist
                        dlLesson.Items.Add(new ListItem((String)rdr[0] + " ~~ " + rdr[1].ToString() + " ~~ " + (String)rdr[2], (String)rdr[0] + "|" + rdr[1].ToString() + "|" + (String)rdr[2]));

                    }//end while
                }
            }
        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

    }

    #endregion Update Country_Grade_Topic

    #region Update Lessons

    private void UpdateLessons(MySqlConnection connection)
    {
        GridLessons(connection);
        try
        {
            connection.Open();

            String sql = "SELECT cid,gid,tid,lid,text,path,filename FROM lessons ORDER BY cid,gid,tid,lid ASC";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
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
                }
            }
        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        bllessons.DataSource = listlesson;
        bllessons.DataBind();
    }

    #endregion Update Lessons

    #endregion UpdateData

    #region Regex

    private bool Regex_TopicCheck(string topic)
    {
        //alphanumeric and [,-:]
        String TopicRegex = @"^([a-zA-Z0-9,\-:]{1}[ ]?){1,49}[a-zA-Z0-9]{1}$";

        if (!Regex.IsMatch(topic, TopicRegex))
        {
            return false;
        }

        return true;
    }

    private bool Regex_CountryCheck(string topic)
    {
        //alphabetic and [-]
        String CountryRegex = @"^([a-zA-Z\-]{1}[ ]?){1,29}[a-zA-Z]{1}$";

        if (!Regex.IsMatch(topic, CountryRegex))
        {
            return false;
        }

        return true;
    }

    private bool Regex_LidCheck(string topic)
    {
        //alphanumeric and [,-:]
        String LidRegex = @"^([a-zA-Z0-9,\-:]{1}[ ]?){1,99}[a-zA-Z0-9]{1}$";

        if (!Regex.IsMatch(topic, LidRegex))
        {
            return false;
        }

        return true;
    }

    private bool Regex_FilenameCheck(string topic)
    {
        //alphanumeric and [_]
        String FilenameRegex = @"^([a-zA-Z0-9_]{1,47})((\.mp3)|(\.MP3))$";

        if (!Regex.IsMatch(topic, FilenameRegex))
        {
            return false;
        }

        return true;
    }

    #endregion Regex

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

    #region DropLists Methods

    private void BlanksOnDropList_WithText(DropDownList ddl, string text)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(text, String.Empty));
        ddl.SelectedIndex = 0;
    }

    private void BlanksOnDropList(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(String.Empty, String.Empty));
        ddl.SelectedIndex = 0;
    }

    private void BlanksOnAllDropLists_WithText()
    {
        //initiate dropdownlists to have a blank

        //for country_grade
        dlCGcountry.Items.Clear();
        dlCGcountry.Items.Add(new ListItem(DROPLIST_COUNTRY_TEXT, String.Empty));
        dlCGcountry.SelectedIndex = 0;

        dlCGgrade.Items.Clear();
        dlCGgrade.Items.Add(new ListItem(DROPLIST_GRADE_TEXT, String.Empty));
        dlCGgrade.SelectedIndex = 0;

        //for country_grade_topic
        dlCGTcountrygrade.Items.Clear();
        dlCGTcountrygrade.Items.Add(new ListItem(DROPLIST_COUNTRY_GRADE_TEXT, String.Empty));
        dlCGTcountrygrade.SelectedIndex = 0;

        dlCGTtopic.Items.Clear();
        dlCGTtopic.Items.Add(new ListItem(DROPLIST_TOPIC_TEXT, String.Empty));
        dlCGTtopic.SelectedIndex = 0;

        //for lesson
        dlLesson.Items.Clear();
        dlLesson.Items.Add(new ListItem(DROPLIST_COUNTRY_GRADE_TOPIC_TEXT, String.Empty));
        dlLesson.SelectedIndex = 0;
        
    }

    #endregion DropLists Methods

    #region Add Methods

    #region AddCountry_Click

    protected void AddCountry_Click(object sender, EventArgs e)
    {

        if (txtcountryAdd.Text.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Country: field not filled";
            return;
        }

        string country = txtcountryAdd.Text;

        if(!Regex_CountryCheck(country))
        {
            tblog.Text += Environment.NewLine + "~Country: Countries can only contain alphabetical letters, spaces, and '-' and size up to 30 characters.";
            return;
        }

        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();
        
            connection.Open();

            String sql = "INSERT INTO countries (cid) VALUES (@cid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);

            cmd.ExecuteNonQuery();

            connection.Close();

            UpdateCountries(connection);

        }//end try
        catch (MySqlException mse)
        {

            good = false;

            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email

        }
        catch (ArgumentException ae)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }
        catch(Exception ex)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        if (good)
        {
            txtcountryAdd.Text = "";
            tblog.Text += Environment.NewLine + "~Country: Successfully Added";
        }

    }//end method

    #endregion AddCountry_Click

    #region AddTopic_Click

    protected void AddTopic_Click(object sender, EventArgs e)
    {

        if (txttopicAdd.Text.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Topic: field not filled.";
            return;
        }

        string topic = txttopicAdd.Text;

        if (!Regex_TopicCheck(topic))
        {
            tblog.Text += Environment.NewLine + "~Topic: Topics can only contain alphanumeric characters, spaces and ',' and '-' and ':' and size up to 50 characters.";
            return;
        }

        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "INSERT INTO topics (tid) VALUES (@tid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@tid", topic);

            cmd.ExecuteNonQuery();

            connection.Close();

            UpdateTopics(connection);

        }//end try
        catch (MySqlException mse)
        {

            good = false;

            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email

        }
        catch (ArgumentException ae)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }
        catch (Exception ex)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        if (good)
        {
            txttopicAdd.Text = "";
            tblog.Text += Environment.NewLine + "~Topic: Successfully Added";
        }

    }//end method

    #endregion AddTopic_Click

    #region Add to CountryGrade

    #region CountryGrade_country_TextChange

    protected void CountryGrade_country_IndexChange(object sender, EventArgs e)
    {

        BlanksOnDropList_WithText(dlCGgrade, DROPLIST_GRADE_TEXT);
        DisableBox(dlCGgrade);

        if (dlCGcountry.SelectedValue.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Country_Grade: country field not filled";
            return;
        }
        
        string country = dlCGcountry.SelectedValue;
        
        bool canContinue = true;

        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            //this selects all the gid from the 'grades' tables that does not yet
            //have an association with the given country
            String sql = "SELECT gid " +
                         "FROM grades " +
                         "WHERE gid NOT IN " +
                            "(SELECT gid FROM country_grade_relationship WHERE cid = (@cid));";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@cid", country);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (!rdr.HasRows)
                    {
                        tblog.Text += Environment.NewLine + "~Country_Grade: no content to add";
                        canContinue = false;
                    }

                    while (rdr.Read())
                    {

                        //add to dlCGgrade
                        dlCGgrade.Items.Add(new ListItem(rdr[0].ToString()));

                    }//end while
                }
            }

            connection.Close();

            if (!canContinue)
            {
                dlCGcountry.SelectedIndex = 0;
            }

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email

        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        dlCGgrade.Enabled = canContinue;

    }

    #endregion CountryGrade_country_TextChange

    #region AddCountryGrade_Click

    protected void AddCountryGrade_Click(object sender, EventArgs e)
    {
        if (dlCGcountry.SelectedValue.Length == 0 || dlCGgrade.SelectedValue.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Country_Grade: not all fields filled";
            return;
        }

        String country = dlCGcountry.SelectedValue;
        String grade = dlCGgrade.SelectedValue;

        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "INSERT INTO country_grade_relationship (cid,gid) VALUES (@cid,@gid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);
            cmd.Parameters.AddWithValue("@gid", grade);

            cmd.ExecuteNonQuery();

            connection.Close();
            
            UpdateCountryGrade(connection);

        }//end try
        catch (MySqlException mse)
        {

            good = false;

            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email

        }
        catch (ArgumentException ae)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }
        catch (Exception ex)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
        
        if (good)
        {
            dlCGcountry.SelectedIndex = 0;
            BlanksOnDropList_WithText(dlCGgrade, DROPLIST_GRADE_TEXT);
            DisableBox(dlCGgrade);
            tblog.Text += Environment.NewLine + "~Country_Grade: Successfully Added";
        }

    }

    #endregion AddCountryGrade_Click

    #endregion Add to CountryGrade

    #region Add to CountryGradeTopic

    #region CountryGradeTopic_countrygrade_TextChange

    protected void CountryGradeTopic_countrygrade_IndexChange(object sender, EventArgs e)
    {

        BlanksOnDropList_WithText(dlCGTtopic, DROPLIST_TOPIC_TEXT);
        DisableBox(dlCGTtopic);

        if (dlCGTcountrygrade.SelectedValue.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Country_Grade_Topic: country_grade field filled.";
            return;
        }
        
        string tempCountryGrade = dlCGTcountrygrade.SelectedValue;
        string[] countrygradeSplit = tempCountryGrade.Split('|');
        string country = countrygradeSplit[0];
        string grade = countrygradeSplit[1];
        
        bool canContinue = true;

        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            //this selects all the tid from the 'topics' table that does not yet
            //have an association with the given country_grade
            String sql = "SELECT tid " +
                         "FROM topics " +
                         "WHERE tid NOT IN " +
                            "(SELECT tid FROM country_grade_topic_relation WHERE cid = (@cid) AND gid = (@gid));";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@cid", country);
                cmd.Parameters.AddWithValue("@gid", grade);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (!rdr.HasRows)
                    {
                        tblog.Text += Environment.NewLine + "Country Grade Topic: no content to add";
                        canContinue = false;
                    }

                    while (rdr.Read())
                    {

                        //add to dlCGgrade
                        dlCGTtopic.Items.Add(new ListItem(rdr[0].ToString()));

                    }//end while
                }
            }

            connection.Close();

            if (!canContinue)
            {
                dlCGTcountrygrade.SelectedIndex = 0;
            }

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email

        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
        
        dlCGTtopic.Enabled = canContinue;

    }

    #endregion CountryGradeTopic_countrygrade_TextChange

    #region AddCountryGradeTopic_Click

    protected void AddCountryGradeTopic_Click(object sender, EventArgs e)
    {
        if (dlCGTcountrygrade.SelectedValue.Length == 0 || dlCGTtopic.SelectedValue.Length == 0)
        {
            tblog.Text += Environment.NewLine + "Country_Grade_Topic: not all fields filled";
            return;
        }

        string tempCountryGrade = dlCGTcountrygrade.SelectedValue;
        string[] countrygradeSplit = tempCountryGrade.Split('|');
        string country = countrygradeSplit[0];
        string grade = countrygradeSplit[1];
        string topic = dlCGTtopic.Text;

        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "INSERT INTO country_grade_topic_relation (cid,gid,tid) VALUES (@cid,@gid,@tid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@cid", country);
                cmd.Parameters.AddWithValue("@gid", grade);
                cmd.Parameters.AddWithValue("@tid", topic);

                cmd.ExecuteNonQuery();

                connection.Close();
                
                UpdateCountryGradeTopic(connection);

            }

        }//end try
        catch (MySqlException mse)
        {

            good = false;

            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email

        }
        catch (ArgumentException ae)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }
        catch (Exception ex)
        {

            good = false;

            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        if (good)
        {
            dlCGTcountrygrade.SelectedIndex = 0;
            BlanksOnDropList_WithText(dlCGTtopic, DROPLIST_TOPIC_TEXT);
            DisableBox(dlCGTtopic);
            tblog.Text += Environment.NewLine + "~Country_Grade_Topic: Successfully Added";
        }

    }

    #endregion AddCountryGradeTopic_Click

    #endregion Add to CountryGradeTopic

    #region AddLesson_Click

    /*
     * credit: http://asp.net-tutorials.com/controls/file-upload-control/
     * credit: https://stackoverflow.com/questions/1762157/how-to-delete-a-file-using-asp-net
     */
    protected void AddLesson_Click(object sender, EventArgs e)
    {
        #region TextBox checks and File Checks
        if (dlLesson.SelectedValue.Length == 0 || txtLessonName.Text.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Lesson: not all fields filled";
            return;
        }

        if (!fileMP3.HasFile)
        {
            tblog.Text += Environment.NewLine + "~Lesson: no file uploaded";
            return;
        }

        if(fileMP3.PostedFile.ContentType != "audio/mp3")
        {
            tblog.Text += Environment.NewLine + "~Lesson: file was not an mp3";
            return;
        }

        if (fileMP3.PostedFile.ContentLength > FILE_SIZE_MAX)
        {
            tblog.Text += Environment.NewLine + "~Lesson: file too large (>15MB (15728640 bytes))";
            return;
        }
        #endregion TextBox checks and File Checks

        string tempCountryGradeTopic = dlLesson.SelectedValue;
        string[] countrygradetopicSplit = tempCountryGradeTopic.Split('|');
        string country = countrygradetopicSplit[0];
        string grade = countrygradetopicSplit[1];
        string topic = countrygradetopicSplit[2];
        string lid = txtLessonName.Text;
        string lessonText = tbtext.Text.Length == 0 ? "*no words for this lesson*" : tbtext.Text;
        string filename = fileMP3.FileName;

        #region Filename Regex and LID Regex
        if (!Regex_FilenameCheck(filename))
        {
            tblog.Text += Environment.NewLine + "~Lesson: Filename can only contain alphanumeric and '_' and size up to 50 characters with '.mp3 or .MP3' extension.";
            return;
        }

        if(!Regex_LidCheck(lid))
        {
            tblog.Text += Environment.NewLine + "~Lesson: Lesson title can only contain alphanumeric, spaces, and ',' and '-' and ':' and size up to 100 characters.";
            return;
        }
        #endregion Filename Regex and LID Regex

        bool good = true;
        bool canAddToDB = false;
        bool existed = false;

        #region Path Work
        string physicalAudioPath = Server.MapPath(".//Audio//");
        string physicalFilePath = physicalAudioPath + filename;

        string virtualCurrentPath = HttpContext.Current.Request.Url.AbsoluteUri;
        string virtualAudioPath = virtualCurrentPath.Substring(0, virtualCurrentPath.LastIndexOf('/')) + "/Audio/";
        string virtualFilePath = virtualAudioPath + filename;
        #endregion Path Work

        if (!System.IO.File.Exists(physicalFilePath))
        {
            try
            {
                fileMP3.SaveAs(physicalFilePath);
                canAddToDB = true;
            }
            catch (Exception ex)
            {
                canAddToDB = false;
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: could not save file | contact admin";
                return;
            }
        }
        else
        {
            tblog.Text += Environment.NewLine + "~Lesson: File already existed, did not upload again";
            existed = true;
            canAddToDB = true;
        }

        bool successfulInsert = false;

        if (canAddToDB)
        {

            MySqlConnection connection = null;

            try
            {
                connection = GetSqlConnection();

                connection.Open();

                String sql = "INSERT INTO lessons(cid, gid, tid, lid, text, path, filename) VALUES(@cid,@gid,@tid,@lid,@text,@path,@fn)";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@cid", country);
                    cmd.Parameters.AddWithValue("@gid", grade);
                    cmd.Parameters.AddWithValue("@tid", topic);
                    cmd.Parameters.AddWithValue("@lid", lid);
                    cmd.Parameters.AddWithValue("@text", lessonText);
                    cmd.Parameters.AddWithValue("@path", virtualFilePath);
                    cmd.Parameters.AddWithValue("@fn", filename);

                    cmd.ExecuteNonQuery();
                }

                successfulInsert = true;

                connection.Close();

                UpdateLessons(connection);

            }//end try
            catch (MySqlException mse)
            {

                good = false;

                string text = MySqlExceptionNumberHandler(mse.Number);
                tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

                //TODO: email

            }
            catch (ArgumentException ae)
            {

                good = false;

                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
                return;
            }
            catch (Exception ex)
            {

                good = false;

                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: contact admin";
            }

            connection.Close();
            
        }
        
        if (!successfulInsert && !existed)
        {
            try
            {
                System.IO.File.Delete(physicalFilePath);
            }
            catch (Exception ex)
            {
                tblog.Text += Environment.NewLine + "~Error: deleting file complication | contact admin";
                //TODO: email
            }
            tblog.Text += Environment.NewLine + "~Lesson: database insertion failed | contact admin";
            //TODO: email
            return;
        }

        if (good)
        {
            dlLesson.SelectedIndex = 0;
            txtLessonName.Text = "";
            tbtext.Text = "";
            tblog.Text += Environment.NewLine + "~Lesson: Successfully Added";
        }
    }

    #endregion AddLesson_Click

    #endregion Add Methods

    #region Helpers

    #region GetSession

    private bool GetSession()
    {
        if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            if (matching)
                return true;
        }//end else
        return true;
    }//end method

    #endregion GetSession

    #region MySqlExceptionHandler

    private string MySqlExceptionNumberHandler(int exceptionNum)
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
            case 1042:
                return "Cannot resolve server name";
            case 1045:
                return "Invalid username/password, please try again";
            case 1062:
                return "Duplicate Entry";
            default:
                return "number: " + exceptionNum;
        }//end switch
    }

    #endregion MySqlExceptionHandler
    
    protected void Btndeletelink_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageDelete.aspx");
    }//end method

    protected void EmailError(String strmess)
    {
        try
        {
            String mypwd = "";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("yourEmail@gmail.com", mypwd),
                EnableSsl = true
            };
            MailMessage message = new MailMessage("yourEmail@gmail.com", "EmailToSendTo", "Error Occurred" , strmess);
            client.Send(message);
        }//end try
        catch (Exception e)
        {
            tblog.Text += "Email failed to send! " + e.ToString();
        }//end catch
    }//end method

    #endregion Helpers

    #region LessonDisplay

    private void GridLessons(MySqlConnection connection)
    {
        try
        {
            connection.Open();

            String sql = "SELECT cid,gid,tid,lid,text,filename FROM lessons  ORDER BY cid,gid,tid,lid ASC";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            DataTable dt = new DataTable();
            MySqlDataAdapter src = new MySqlDataAdapter(cmd);
            src.Fill(dt);
            gridLesson.DataSource = dt;
            gridLesson.DataBind();

        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
    }

    protected void Lesson_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Attributes.Add("style", "width:770px;word-break:break-all;word-wrap:break-word;");
        }
    }

    #endregion LessonDisplay



}//end class