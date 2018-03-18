using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();
        if(run)
        {
            //all code goes here
            UpdateAllData();
        }//end if
        else
            Response.Redirect("Login.aspx");
    }//end method

    private void UpdateAllData()
    {

        String text = "";

        MySqlConnection connection = GetSqlConnection();

        try
        {

            UpdateCountries(connection);

            UpdateGrades(connection);

            UpdateTopics(connection);

            UpdateLessons(connection);

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        errormsgDB.Text = text;

    }//end method


    private void UpdateCountries(MySqlConnection connection)
    {

        connection.Open();

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
            //dlCGcountry.Items.Add(new ListItem((String)rdr[0], (String)rdr[0]));

        }//end while
        rdr.Close();
        connection.Close();

        blcountry.DataSource = listcountry;
        blcountry.DataBind();
    }//end method


    private void UpdateGrades(MySqlConnection connection)
    {
        connection.Open();

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
    }//end method

    private void UpdateTopics(MySqlConnection connection)
    {
        connection.Open();

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
    }//end method

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
    }//end method



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

    private MySqlConnection GetSqlConnection()
    {

        //string text = "";
        string server = "162.241.244.134";
        string database = "jordape8_EnglishApp";
        string uid = "jordape8_Default";
        string password = "Default1!";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        //local testing >>>
        //connectionString = GetConnectionString();
        //local testing <<<

        return new MySqlConnection(connectionString);
    }//end method

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
    }//end method

    protected void Btnaddlink_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageAdd.aspx");
    }//end method


}//end class