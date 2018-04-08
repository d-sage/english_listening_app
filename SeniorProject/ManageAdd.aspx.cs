using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;

public partial class Manage : System.Web.UI.Page
{
    private int num;
    private String stringhelper;
    private int inthelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();

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

        
        string server = "localhost";
        string database = "daricsag_ela";
        string uid = "daricsag_ela";
        string password = "english";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        

        /*
        string server = "mysql5018.site4now.net";
        string database = "db_a38d8d_lambe";
        string uid = "a38d8d_lambe";
        string password = "Lambejor000";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */

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
            throw new ArgumentException();
        }

        return conn;
    }

    #endregion Get SQL Connection

    #region UpdateData

    #region Update All Data

    private void UpdateAllData()
    {

        BlanksOnAllDropLists();

        DisableBoxes();

        String text = "Good";
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
        
        errormsgDB.Text = text; //TODO: display to the messagebox

    }

    #endregion Update All Data

    #region Update Countries

    private void UpdateCountries(MySqlConnection connection)
    {

        BlanksOnDropList(dlCGcountry);

        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT * FROM countries";

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

            String sql = "SELECT * FROM topics";

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

        BlanksOnDropList(dlCGTcountrygrade);

        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT * FROM country_grade_relationship";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {

                        //add to countrygrade droplist
                        dlCGTcountrygrade.Items.Add(new ListItem((String)rdr[0] + " " + rdr[1].ToString(), (String)rdr[0] + " " + rdr[1].ToString()));

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

        BlanksOnDropList(dlLesson);

        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT * FROM country_grade_topic_relation";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {

                        //add to countrygrade droplist
                        dlLesson.Items.Add(new ListItem((String)rdr[0] + " " + rdr[1].ToString() + " " + (String)rdr[2], (String)rdr[0] + " " + rdr[1].ToString() + " " + (String)rdr[2]));

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

        try
        {
            connection.Open();

            String sql = "SELECT * FROM lessons";

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

        //for lesson
        dlLesson.Items.Clear();
        dlLesson.Items.Add(new ListItem(String.Empty, String.Empty));
        dlLesson.SelectedIndex = 0;



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
            tblog.Text += Environment.NewLine + "~Country: field not filled";
            return;
        }

        string country = txtcountryAdd.Text;

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
        
        txtcountryAdd.Text = "";

        if (good)
            tblog.Text += Environment.NewLine + "~Country: Successfully Added";

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
        
        txttopicAdd.Text = "";

        if (good)
            tblog.Text += Environment.NewLine + "~Topic: Successfully Added";

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
            tblog.Text += Environment.NewLine + "~Country_Grade: country field not filled";
            return;
        }
        
        string country = dlCGcountry.Text;
        
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
        
        if (!canContinue)
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
            tblog.Text += Environment.NewLine + "~Country_Grade: not all fields filled,";
            return;
        }

        String country = dlCGcountry.Text;
        String grade = dlCGgrade.Text;

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
        
        UpdateAllData();

        if(good)
            tblog.Text += Environment.NewLine + "~Country_Grade: Successfully Added";

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
            tblog.Text += Environment.NewLine + "~Country_Grade_Topic: country_grade field filled.";
            return;
        }
        
        string tempCountryGrade = dlCGTcountrygrade.Text;
        string[] countrygradeSplit = tempCountryGrade.Split(' ');
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
                        ClientScript.RegisterStartupScript(this.GetType(), "Invalid Entry", "alert('Country Grade Topic: no content to add');", true);
                        canContinue = false;
                    }

                    while (rdr.Read())
                    {

                        //add to dlCGgrade
                        dlCGTtopic.Items.Add(new ListItem(rdr[0].ToString()));

                    }//end while
                }
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
            tblog.Text += Environment.NewLine + "Country_Grade_Topic: not all fields filled";
            return;
        }

        string tempCountryGrade = dlCGTcountrygrade.Text;
        string[] countrygradeSplit = tempCountryGrade.Split(' ');
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
        
        UpdateAllData();

        if(good)
            tblog.Text += Environment.NewLine + "~Country_Grade_Topic: Successfully Added";

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
        if (dlLesson.Text.Length == 0 || txtLessonName.Text.Length == 0)
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

        if (fileMP3.PostedFile.ContentLength > 512000)     //500KB = 500 * 1024
        {
            tblog.Text += Environment.NewLine + "~Lesson: file too large (>500KB)";
            return;
        }





        //TODO: restricct all additions to size of database columns


        


        string tempCountryGradeTopic = dlLesson.Text;
        string[] countrygradetopicSplit = tempCountryGradeTopic.Split(' ');
        string country = countrygradetopicSplit[0];
        string grade = countrygradetopicSplit[1];
        string topic = countrygradetopicSplit[2];
        string name = txtLessonName.Text;
        string lessonText = tbtext.Text.Length == 0 ? "*no words for this lesson*" : tbtext.Text;
        string filename = fileMP3.FileName;

        if (filename.Length > 50)     //500KB = 500 * 1024
        {
            tblog.Text += Environment.NewLine + "~Lesson: filename+extension cannot be larger than 50 characters.";
            return;
        }

        bool good = true;
        bool canAddToDB = false;
        bool existed = false;

        string physicalAudioPath = Server.MapPath(".//Audio//");
        string physicalFilePath = physicalAudioPath + filename;

        string virtualCurrentPath = HttpContext.Current.Request.Url.AbsoluteUri;
        string virtualAudioPath = virtualCurrentPath + "/Audio/";
        string virtualFilePath = virtualAudioPath + filename;
        
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
                    cmd.Parameters.AddWithValue("@lid", name);
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

        //TODO: try-cacth
        if (!successfulInsert && !existed)
        {
            try
            {
                System.IO.File.Delete(physicalFilePath);
            }
            catch (Exception ex)
            {
                tblog.Text += Environment.NewLine + "~Error: deleting file complication | contact admin";
            }
            tblog.Text += Environment.NewLine + "~Lesson: database insertion failed | contact admin";
            return;
        }
        
        if(good)
            tblog.Text += Environment.NewLine + "~Lesson: Successfully Added";
    }

    #endregion AddLesson_Click

    #region GetSession

    private bool GetSession()
    {/*
        if((Session["confirm"]) == null)
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
    
}//end class