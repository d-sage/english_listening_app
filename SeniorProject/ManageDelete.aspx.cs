using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageDelete : System.Web.UI.Page
{
    private String stringhelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();


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


        return connectionString;
    }

    #endregion GetConnectionString

    #region Get SQL Connection

    private MySqlConnection GetSqlConnection()
    {
        /*
        string text = "";
        string server = "162.241.244.134";
        string database = "jordape8_EnglishApp";
        string uid = "jordape8_admin";
        string password = "Admin123";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */

        //local testing >>>
        string connectionString = GetConnectionString();
        //local testing <<<

        return new MySqlConnection(connectionString);
    }

    #endregion Get SQL Connection

    #region UpdateData

    #region Update All Data

    private void UpdateAllData()
    {

        String text = "Good";

        MySqlConnection connection = GetSqlConnection();

        try
        {

            UpdateCountries(connection);

            UpdateGrades(connection);

            UpdateTopics(connection);

            UpdateCountryGrade(connection);

            UpdateCountryGradeTopic(connection);

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

        //display the countries
        String sql = "SELECT cid FROM countries";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        /*MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            //add to bulletlist display
            stringhelper = (String)rdr[0];
            listcountry.Add(stringhelper);

            //add to countries to table
            //dlCGcountry.Items.Add(new ListItem((String)rdr[0], (String)rdr[0]));

        }//end while*/



        //test
        DataTable dt = new DataTable();
        MySqlDataAdapter src = new MySqlDataAdapter(cmd);
        src.Fill(dt);
        gridCountry.DataSource = dt;
        gridCountry.DataBind();
        //test




        //rdr.Close();
        connection.Close();

        //databind countries to bullet lists
        //blcountry.DataSource = listcountry;
        //blcountry.DataBind();

        //databind countries to table
        //gridCountry.DataSource = listcountry;
        //gridCountry.DataBind();
        
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
        /*MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            stringhelper = (String)rdr[0];
            listtopic.Add(stringhelper);
        }//end while*/


        //test
        DataTable dt = new DataTable();
        MySqlDataAdapter src = new MySqlDataAdapter(cmd);
        src.Fill(dt);
        gridTopic.DataSource = dt;
        gridTopic.DataBind();
        //test


        //rdr.Close();
        connection.Close();

        //bltopics.DataSource = dt;
        //bltopics.DataBind();
    }

    #endregion Update Topics

    #region Update Country_Grade

    private void UpdateCountryGrade(MySqlConnection connection)
    {

        connection.Open();

        //display the countries
        String sql = "SELECT * FROM country_grade_relationship";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {

            //add to countrygrade droplist
            //dlCGTcountrygrade.Items.Add(new ListItem((String)rdr[0] + " " + rdr[1].ToString(), (String)rdr[0] + " " + rdr[1].ToString()));

        }//end while
        rdr.Close();
        connection.Close();

    }

    #endregion Update Country_Grade

    #region Update Country_Grade_Topic

    private void UpdateCountryGradeTopic(MySqlConnection connection)
    {

        connection.Open();

        //display the countries
        String sql = "SELECT * FROM country_grade_topic_relation";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {

            //add to countrygrade droplist
            //dlLesson.Items.Add(new ListItem((String)rdr[0] + " " + rdr[1].ToString() + " " + (String)rdr[2], (String)rdr[0] + " " + rdr[1].ToString() + " " + (String)rdr[2]));

        }//end while
        rdr.Close();
        connection.Close();

    }

    #endregion Update Country_Grade_Topic

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

    #region Table Delete Events

    #region Delete Country

    /*
     *  Cannot delete a country if there is any data that is associated with this given country
     *  Then will update country data
     */
    protected void Country_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        GridViewRow row = gridCountry.Rows[int.Parse(e.CommandArgument.ToString())];
        string countryToDelete = row.Cells[0].Text;

        string text = "Good";
        bool canRemove = false;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT * FROM country_grade_relationship WHERE cid = (@cid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", countryToDelete);

            MySqlDataReader rdr = cmd.ExecuteReader();

            //if there are rows present, then cannot remove
            canRemove = !rdr.HasRows;
            
            rdr.Close();

            if(canRemove)
            {
                sql = "DELETE FROM countries WHERE cid = (@cid)";

                cmd.CommandText = sql;
                
                cmd.ExecuteNonQuery();

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country: content associateed with Country, cannot remove');", true);
            }

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;
        

        if (canRemove)
        {
            UpdateCountries(GetSqlConnection());
        }

    }

    #endregion Delete Country

    #region Delete Topic

    /*
     *  Cannot delete a topic if there is any data that is associated with this given topic
     *  Then will update topic data
     */
    protected void Topic_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        GridViewRow row = gridTopic.Rows[int.Parse(e.CommandArgument.ToString())];
        string topicToDelete = row.Cells[0].Text;

        string text = "Good";
        bool canRemove = false;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT * FROM country_grade_topic_relation WHERE tid = (@tid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@tid", topicToDelete);

            MySqlDataReader rdr = cmd.ExecuteReader();

            //if there are rows present, then cannot remove
            canRemove = !rdr.HasRows;

            rdr.Close();

            if (canRemove)
            {
                sql = "DELETE FROM topics WHERE tid = (@tid)";

                cmd.CommandText = sql;
                
                cmd.ExecuteNonQuery();

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Topic: content associateed with Topic, cannot remove');", true);
            }

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;


        if (canRemove)
        {
            UpdateTopics(GetSqlConnection());
        }

    }

    #endregion Delete Topic

    #endregion Table Delete Events

    #region Table Edit Events

    #endregion Table Edit Events

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

    #region GetSession()

    private bool GetSession()
    {
        /*if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            //num = (int)Session["number"];
            if (matching)
                return true;
        }//end else*/
        return true;
    }//end method

    #endregion GetSession()

    protected void Btnaddlink_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageAdd.aspx");
    }//end method


}//end class