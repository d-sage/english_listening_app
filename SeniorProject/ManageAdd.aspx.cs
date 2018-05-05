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
    /*
     * 
     * TODO
     * 
     */
    private int num;
    private String stringhelper;
    private int inthelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();
    
    /*
     * 
     * Constants used throughout the ManageAdd page for constraints
     * related to the fields in the database
     * 
     */
    private const int DB_COUNTRY_LENGTH_MAX = 30;
    private const int DB_TOPIC_LENGTH_MAX = 50;
    private const int DB_LID_LENGTH_MAX = 100;
    private const int DB_TEXT_LENGTH_MAX = 2500;
    private const int FILE_NAME_LENGTH_MAX = 50;
    private const int FILE_SIZE_MAX = 15728640;

    /*
     * 
     * Constants used for inserting text in to dropboxes so that these
     * constants can be reused and intent revealing
     * 
     */
    private const string DROPLIST_COUNTRY_TEXT = "country...";
    private const string DROPLIST_TOPIC_TEXT = "topic...";
    private const string DROPLIST_GRADE_TEXT = "grade...";
    private const string DROPLIST_COUNTRY_GRADE_TEXT = "country grade...";
    private const string DROPLIST_COUNTRY_GRADE_TOPIC_TEXT = "country grade topic...";
    

    #region Page_Load
    /*
     * 
     * This will check the load in session to see if they passed the login 
     * This also checks for a page refresh and then redirects them back to the page instead of reload it
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();
        bool isPageRefreshed = false;
        //makes sure they aren't going around the login
        if (run)
        {
            if (!IsPostBack)
            {
                ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                Session["SessionId"] = ViewState["ViewStateId"].ToString();
            }
            else
            {
                if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
                {
                    isPageRefreshed = true;
                    Response.Redirect("ManageAdd.aspx");
                }

                Session["SessionId"] = System.Guid.NewGuid().ToString();
                ViewState["ViewStateId"] = Session["SessionId"].ToString();
            }
            if (!Page.IsPostBack)
            {
                Session["ee"] = "";
                UpdateAllData();
            }
            
        }//end if
        else
            Response.Redirect("Login.aspx");

    }//end main method

    #endregion Page_Load

    #region GetConnectionString
    /*
     * 
     * Method that contains the variables to construct the connection
     * string required to access the database.
     * 
     */
    private string GetConnectionString()
    {
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
    /*
     * 
     * Method that attempts to get a database connection object
     * from the connection string and return it for database
     * access
     * 
     */
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
            Session["ee"] = Environment.NewLine + "~" + e.Message;
            throw new ArgumentException();
        }

        return conn;
    }

    #endregion Get SQL Connection

    #region UpdateData

    #region Update All Data
    /*
     * 
     * Method that puts the ManageAdd page back to a 'blank slate' state
     * in which droplists are disabled and inserted with default text, and
     * all data from the database is re-initialized
     * 
     */
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
            Session["ee"] = Environment.NewLine + "~" + "SQL Connection Failed";
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            return;
        }

        if (!UpdateCountries(connection))
            return;

        if (!UpdateGrades(connection))
            return;

        if (!UpdateTopics(connection))
            return;

        if (!UpdateCountryGrade(connection))
            return;

        if (!UpdateCountryGradeTopic(connection))
            return;

        if (!UpdateLessons(connection))
            return;
        
    }

    #endregion Update All Data

    #region Update Countries
    /*
     * 
     * Method for updating data related to 'countries'. Reverting 'country'
     * droplist back to original state and re-populating it with the current
     * 'country data' from the database.
     * 
     */
    private bool UpdateCountries(MySqlConnection connection)
    {

        bool good = true;

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
            good = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch(Exception e)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + e.Message;
        }

        connection.Close();

        blcountry.DataSource = listcountry;
        blcountry.DataBind();

        return good;
    }

    #endregion Update Countries

    #region Update Grades
    /*
     * 
     * Loads in the new grades everytime a page load is called
     * 
     */
    private bool UpdateGrades(MySqlConnection connection)
    {

        bool good = true;

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
            good = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch(Exception e)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + e.Message;
        }

        connection.Close();

        return good;
    }

    #endregion Update Grades

    #region Update Topics
    /*
     * 
     * Loads in the new topics everytime a page load is called
     * 
     */
    private bool UpdateTopics(MySqlConnection connection)
    {

        bool good = true;

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
            good = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (Exception e)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + e.Message;
        }

        connection.Close();

        return good;
    }

    #endregion Update Topics

    #region Update Country_Grade
    /*
     * 
     * Method for updating data related to 'country grade'. Reverting
     * 'country grade' droplist back to original state and re-populating
     * it with the current 'country grade' data from the database.
     * 
     */
    private bool UpdateCountryGrade(MySqlConnection connection)
    {

        bool good = true;

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
            good = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (Exception e)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + e.Message;
        }

        connection.Close();

        return good;
    }

    #endregion Update Country_Grade

    #region Update Country_Grade_Topic
    /*
     * 
     * Method for updating data related to 'country grade topic'.
     * Reverting 'country grade topic' droplist back to original
     * state and re-populating it with the current 'country grade topic'
     * data from the database.
     * 
     */
    private bool UpdateCountryGradeTopic(MySqlConnection connection)
    {

        bool good = true;

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
            good = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (Exception e)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + e.Message;
        }

        connection.Close();

        return good;
    }

    #endregion Update Country_Grade_Topic

    #region Update Lessons
    /*
     * 
     * Loads in the new lessons everytime a page load is called
     * 
     */
    private bool UpdateLessons(MySqlConnection connection)
    {

        bool good = true;

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
            good = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (Exception e)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + e.Message;
        }

        connection.Close();

        return good;
    }

    #endregion Update Lessons

    #endregion UpdateData

    #region Regex
    /*
     * 
     * Regex method for checking the validity of characters and
     * length associated with a 'topic'.
     * They can contain: alphanumeric, [,-:], and spaces
     * 
     */
    private bool Regex_TopicCheck(string topic)
    {
        String TopicRegex = @"^([a-zA-Z0-9,\-:]{1}[ ]?){1,49}[a-zA-Z0-9]{1}$";

        if (!Regex.IsMatch(topic, TopicRegex))
        {
            return false;
        }

        return true;
    }

    /*
     * 
     * Regex method for checking the validity of characters and
     * length associated with a 'country'.
     * They can contain: alphabetic, [-], and spaces
     * 
     */
    private bool Regex_CountryCheck(string topic)
    {
        String CountryRegex = @"^([a-zA-Z\-]{1}[ ]?){1,29}[a-zA-Z]{1}$";

        if (!Regex.IsMatch(topic, CountryRegex))
        {
            return false;
        }

        return true;
    }

    /*
     * 
     * Regex method for checking the validity of characters and
     * length associated with a 'lid' (lesson title).
     * They can contain: alphanumeric, [,-:], and spaces
     * 
     */
    private bool Regex_LidCheck(string topic)
    {
        String LidRegex = @"^([a-zA-Z0-9,\-:]{1}[ ]?){1,99}[a-zA-Z0-9]{1}$";

        if (!Regex.IsMatch(topic, LidRegex))
        {
            return false;
        }

        return true;
    }

    /*
     * 
     * Regex method for checking the validity of characters and
     * length associated with a 'filename'.
     * They can contain: alphanumeric, [_], and end in .mp3 or .pdf
     * variations
     * 
     */
    private bool Regex_FilenameCheck(string topic)
    {
        //alphanumeric and [_]
        String FilenameRegex = @"^([a-zA-Z0-9_]{1,47})\.(mp3|MP3|pdf|PDF)$";

        if (!Regex.IsMatch(topic, FilenameRegex))
        {
            return false;
        }

        return true;
    }

    #endregion Regex

    #region Disable Boxes

    /*
     * 
     * Method for disabling all neccessary droplists
     * on the page for default state
     * 
     */
    private void DisableBoxes()
    {

        //for country_grade add
        dlCGgrade.Enabled = false;

        //for country_grade_topic add
        dlCGTtopic.Enabled = false;

    }

    /*
     * 
     * Method for disabling a specified droplist
     * 
     */
    private void DisableBox(DropDownList ddl)
    {
        ddl.Enabled = false;
    }

    #endregion Disable Boxes

    #region DropLists Methods
    /*
     * 
     * Method for clearing a specified droplist, setting specified
     * text to it, and focusing on index '0'
     * 
     */
    private void BlanksOnDropList_WithText(DropDownList ddl, string text)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(text, String.Empty));
        ddl.SelectedIndex = 0;
    }

    /*
     * 
     * Method for clearing a specified droplist, setting the text
     * to an empty string, and focusing on index '0'
     * 
     */
    private void BlanksOnDropList(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(String.Empty, String.Empty));
        ddl.SelectedIndex = 0;
    }

    /*
     * 
     * Method for clearing all droplists on the ManageAdd page and setting
     * them all to default state with default text, and focusing on index '0'
     * 
     */
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
    /*
     * 
     * Event associated with clicking the 'add country' button.
     * It checks for the field being filled and a regex check.
     * Then attempts to insert the country in to the database.
     * If successful, then the country data will update.
     * If failure occurs, log messages will be produced and
     * an email will be sent.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Country: Countries can only contain: alphabetical characters, spaces, and dashes. Max length = up to 30 characters.";
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
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (ArgumentException ae)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            Session["ee"] = Environment.NewLine + "~" + "SQL Connection Failed";
            return;
        }
        catch(Exception ex)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + ex.Message;
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
    /*
     * 
     * Event associated with clicking the 'add topic' button.
     * It checks for the field being filled and a regex check.
     * Then attempts to insert the topic in to the database.
     * If successful, then the country data will update.
     * If failure occurs, log messages will be produced and
     * an email will be sent.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Topic: Topics can only contain: alphanumeric characters, spaces, commas, dashes and colons. Max length = up to 50 characters.";
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
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (ArgumentException ae)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            Session["ee"] = Environment.NewLine + "~" + "SQL Connection Failed";
            return;
        }
        catch (Exception ex)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + ex.Message;
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
    /*
     * 
     * Event associated with selecting an item from the 'country' droplist.
     * It puts the grade droplist in to a default state to ensure it is at
     * a known state always (in case of bad selection on the country droplist).
     * It checks for the field being filled.
     * Then attempts to check that there are grades that can still be associated
     * with that given country.
     * If okay, then the grades will populate the grade droplist
     * If failure occurs, log messages will be produced and
     * an email may be sent.
     * 
     */
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
                        tblog.Text += Environment.NewLine + "~Country_Grade: all grades are associated with " + country + ", no more grades to add";
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
            canContinue = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (ArgumentException ae)
        {
            canContinue = false;
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            Session["ee"] = Environment.NewLine + "~" + "SQL Connection Failed";
            return;
        }
        catch (Exception ex)
        {
            canContinue = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + ex.Message;
        }

        connection.Close();

        dlCGgrade.Enabled = canContinue;

    }

    #endregion CountryGrade_country_TextChange

    #region AddCountryGrade_Click
    /*
     * 
     * Event associated with clicking the 'add country grade' button.
     * It checks for the fields being filled.
     * Then attempts to insert the country grade in to the database.
     * If successful, then the country grade data will update.
     * If failure occurs, log messages will be produced and
     * an email will be sent.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (ArgumentException ae)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            Session["ee"] = Environment.NewLine + "~" + "SQL Connection Failed";
            return;
        }
        catch (Exception ex)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + ex.Message;
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
    /*
     * 
     * Event associated with selecting an item from the 'country grade' droplist.
     * It puts the topic droplist in to a default state to ensure it is at
     * a known state always (in case of bad selection on the country grade droplist).
     * It checks for the field being filled.
     * Then attempts to check that there are topics that can still be associated
     * with that given country grade.
     * If okay, then the topics will populate the topic droplist
     * If failure occurs, log messages will be produced and
     * an email may be sent.
     * 
     */
    protected void CountryGradeTopic_countrygrade_IndexChange(object sender, EventArgs e)
    {

        BlanksOnDropList_WithText(dlCGTtopic, DROPLIST_TOPIC_TEXT);
        DisableBox(dlCGTtopic);

        if (dlCGTcountrygrade.SelectedValue.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Country_Grade_Topic: country_grade field not filled.";
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
                        tblog.Text += Environment.NewLine + "Country Grade Topic: all topics are associated with " + country + "~~" + grade + ", no more topics to add";
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
            canContinue = false;
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (ArgumentException ae)
        {
            canContinue = false;
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            Session["ee"] = Environment.NewLine + "~" + "SQL Connection Failed";
            return;
        }
        catch (Exception ex)
        {
            canContinue = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + ex.Message;
        }

        connection.Close();
        
        dlCGTtopic.Enabled = canContinue;

    }

    #endregion CountryGradeTopic_countrygrade_TextChange

    #region AddCountryGradeTopic_Click
    /*
     * 
     * Event associated with clicking the 'add country grade topic' button.
     * It checks for the fields being filled.
     * Then attempts to insert the country grade topic in to the database.
     * If successful, then the country grade topic data will update.
     * If failure occurs, log messages will be produced and
     * an email will be sent.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (ArgumentException ae)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            Session["ee"] = Environment.NewLine + "~" + "SQL Connection Failed";
            return;
        }
        catch (Exception ex)
        {
            good = false;
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + ex.Message;
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
    * 
    * Event associated with clicking the 'add lesson' button.
    * It checks for the fields being filled, a file is uploaded, the
    * uploaded file is of correct form (type and size).
    * It does a Regex check on the filename and the lid.
    * Then attempts to get path data (physical and virtual) for the database and file upload.
    * Then attempts to save the file on the server.
    * If successful, then will attempt to insert the lesson data in to the database.
    * If failure occurs anywhere, database/server state is reverted back to the original state
    * and log messages will be produced and an email will be sent.
    * 
    */
    protected void AddLesson_Click(object sender, EventArgs e)
    {

        string ext = "";

        #region TextBox checks and File Checks
        if (dlLesson.SelectedValue.Length == 0 || txtLessonName.Text.Length == 0)
        {
            tblog.Text += Environment.NewLine + "~Lesson: not all fields filled";
            return;
        }

        if (!fileUpload.HasFile)
        {
            tblog.Text += Environment.NewLine + "~Lesson: no file uploaded";
            return;
        }

        if(fileUpload.PostedFile.ContentType == "audio/mp3")
        {
            ext = "mp3";
        }
        else if(fileUpload.PostedFile.ContentType == "application/pdf")
        {
            ext = "pdf";
        }
        else
        {
            tblog.Text += Environment.NewLine + "~Lesson: file was not an mp3 or pdf";
            return;
        }

        if (fileUpload.PostedFile.ContentLength > FILE_SIZE_MAX)
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
        string lessonText = tbtext.Text.Length == 0 ? "*no text for this lesson*" : tbtext.Text;
        string filename = fileUpload.FileName;

        #region Filename Regex and LID Regex
        if (!Regex_FilenameCheck(filename))
        {
            tblog.Text += Environment.NewLine + "~Lesson: Filename can only contain: alphanumeric characters and dashes. Max length = up to 50 characters with '.mp3 or .pdf' extension.";
            return;
        }

        if(!Regex_LidCheck(lid))
        {
            tblog.Text += Environment.NewLine + "~Lesson: Lesson title can only contain: alphanumeric characters, spaces, commas, dashes, and colons. Max length = up to 100 characters.";
            return;
        }
        #endregion Filename Regex and LID Regex

        bool good = true;
        bool canAddToDB = false;
        bool existed = false;

        #region Path Work
        string physicalFolderPath = ext == "mp3" ? Server.MapPath(".//Audio//") : Server.MapPath(".//PDF//");
        string physicalFilePath = physicalFolderPath + filename;

        string virtualCurrentPath = HttpContext.Current.Request.Url.AbsoluteUri;
        string virtualAudioPath = virtualCurrentPath.Substring(0, virtualCurrentPath.LastIndexOf('/')) + (ext == "mp3" ? "/Audio/" : "/PDF/");
        string virtualFilePath = virtualAudioPath + filename;
        #endregion Path Work

        //check for file exists
        //if not, then attempt to save it
        if (!System.IO.File.Exists(physicalFilePath))
        {
            try
            {
                fileUpload.SaveAs(physicalFilePath);
                canAddToDB = true;
            }
            catch (Exception ex)
            {
                canAddToDB = false;
                tblog.Text += Environment.NewLine + "~Error: could not save/upload file | contact help";
                Session["ee"] = Environment.NewLine + "~" + "Could not save file" + Environment.NewLine + "~" + ex.Message;
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

                String sql = "INSERT INTO lessons(cid, gid, tid, lid, text, path, filename, ext) VALUES(@cid,@gid,@tid,@lid,@text,@path,@fn,@ext)";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@cid", country);
                    cmd.Parameters.AddWithValue("@gid", grade);
                    cmd.Parameters.AddWithValue("@tid", topic);
                    cmd.Parameters.AddWithValue("@lid", lid);
                    cmd.Parameters.AddWithValue("@text", lessonText);
                    cmd.Parameters.AddWithValue("@path", virtualFilePath);
                    cmd.Parameters.AddWithValue("@fn", filename);
                    cmd.Parameters.AddWithValue("@ext", ext);

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
                tblog.Text += Environment.NewLine + text;
                Session["ee"] = Environment.NewLine + "~" + mse.Message;
            }
            catch (ArgumentException ae)
            {
                good = false;
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
                Session["ee"] = Environment.NewLine + "~" + "SQL Connecction Failed";
                return;
            }
            catch (Exception ex)
            {
                good = false;
                tblog.Text += Environment.NewLine + "~Error: contact help";
                Session["ee"] = Environment.NewLine + "~" + ex.Message;
            }

            connection.Close();
            
        }
        
        //if the insert failed and the file did not exist originally
        //then remove the file that was uploaded
        if (!successfulInsert && !existed)
        {
            try
            {
                System.IO.File.Delete(physicalFilePath);
            }
            catch (Exception ex)
            {
                tblog.Text += Environment.NewLine + "~Error: deleting file complication | contact help";
                Session["ee"] = Environment.NewLine + "~" + "deleting file complication" + Environment.NewLine + "~" + ex.Message;
            }
            tblog.Text += Environment.NewLine + "~Lesson: database insertion failed | contact help";
            Session["ee"] = Environment.NewLine + "~" + "Lesson DataBase Insertion Failed";
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

    #region Clear Log
    /*
     * 
     * Event assocatied with clicking the 'clear log' button
     * to revert the log box back to a clean slate
     * 
     */
    protected void ClearLog_Click(object sender, EventArgs e)
    {
        tblog.Text = "";
    }

    #endregion Clear Log

    #region GetSession
    /*
     * 
     * This checks the session to see if the login was passed
     * if it was we will load the page if not then takes them back to the login page
     */
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
    /*
     * 
     * Method used for determining what the sql error number corresponds to.
     * It will return the text associated with that error
     * 
     */
    private string MySqlExceptionNumberHandler(int exceptionNum)
    {
        switch (exceptionNum)
        {
            case 0:
                return "~Error: Cannot connect to server | Contact help";
            case 1042:
                return "~Error: Cannot resolve server name  | Contact help";
            case 1045:
                return "~Error: Invalid username/password | Contact help";
            case 1062:
                return "~Duplicate Entry (certain fields cannot be the same), please try again";
            default:
                return "~Error: number: " + exceptionNum + " | Contact help";
        }//end switch
    }

    #endregion MySqlExceptionHandler

    #region LessonDisplay

    /*
     * 
     * Method for displaying the current 'Lesson' data in the database
     * for the admin to easiy view the contents
     * 
     */
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
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;
            Session["ee"] = Environment.NewLine + "~" + mse.Message;
        }
        catch (Exception e)
        {
            tblog.Text += Environment.NewLine + "~Error: contact help";
            Session["ee"] = Environment.NewLine + "~" + e.Message;
        }

        connection.Close();
    }

    protected void Lesson_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Taken out: word-break:break-all;word-wrap:break-word;
            e.Row.Cells[4].Attributes.Add("style", "width:770px;");
        }
    }

    #endregion LessonDisplay

    #region Email Methods
    /*
     * 
     * This will create an email everytime an error occurs with connection to the database
     * It will send the email and the error message to the email specified 
     */
    protected void EmailError()
    {

        if(((string)Session["ee"]).Length <= 0)
        {
            return;
        }

        try
        {
            String mypwd = "Nicaragua2017";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("reachingforenglish@gmail.com", mypwd),
                EnableSsl = true
            };
            MailMessage message = new MailMessage("reachingforenglish@gmail.com", "reachingforenglish@gmail.com", "Error Occurred - ManageAdd" , (string)Session["ee"]);
            client.Send(message);
        }//end try
        catch (Exception e)
        {
            tblog.Text += Environment.NewLine + "Email failed to send! " + e.ToString();
        }//end catch

        Session["ee"] = "";

    }//end method

    protected void EmailTimerTick(object sender, EventArgs e)
    {
        EmailError();
    }

    #endregion Email Methods

    #endregion Helpers

    #region Button Click Events
    /*
     * 
     * Redirects them to the delete page if clicked
     * 
     */
    protected void Btndeletelink_Click(object sender, EventArgs e)
    {
        EmailError();
        Response.Redirect("ManageDelete.aspx");
    }//end method


    /*
     * 
     * Redirects them to the PDF Viewer page if clicked
     * 
     */
    protected void BtnPdf_Click(object sender, EventArgs e)
    {
        EmailError();
        Response.Redirect("PDFViewer.aspx");
    }//end method


    protected void Btnlogout_Click(object sender, EventArgs e)
    {
        EmailError();
        Session["confirm"] = false;
        Response.Redirect("Login.aspx");
    }//end method

    #endregion Button Click Events



}//end class