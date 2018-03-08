using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manage : System.Web.UI.Page
{
    private int num;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();
        //makes sure they aren't going around the login
        if(run)
        {
            ValueHiddenField.Value = num.ToString();

            if(!SetAjaxSession())
            {
                //TODO: did not set correctly
                //TODO: handle
            }

            //*all code goes in here*

        }//end if
        else
            Response.Redirect("Login.aspx");

    }//end main method

    protected void Page_Unload(object sender, EventArgs e)
    {
        
        //TODO: delete session stuff

        //!!not correct event, this gets called right after 'load' event!!

    }

    protected void Submit_Click(object sender, EventArgs e)
    {

    }//end method

    private bool GetSession()
    {
        if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            num = (int)Session["number"];
            if (matching)
                return true;
        }//end else
        return false;
    }//end method

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

        Label1.Text = text;

        return isGood;
    }


}//end class